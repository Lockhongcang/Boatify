using Boatify.Models;
using Boatify.Models.Dto;

namespace Boatify.Services
{
    /// <summary>
    /// Service to handle integration between simplified internal models and external API
    /// </summary>
    public class OrderIntegrationService
    {
        private readonly BookingService _bookingService;
        private readonly ILogger<OrderIntegrationService> _logger;
        private readonly IConfiguration _configuration;

        public OrderIntegrationService(
            BookingService bookingService, 
            ILogger<OrderIntegrationService> logger,
            IConfiguration configuration)
        {
            _bookingService = bookingService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Convert internal order to external API format and create order
        /// </summary>
        public async Task<CreateOrderResponse> CreateExternalOrderAsync(Order order, List<Ticket> tickets)
        {
            try
            {
                var enableExternalApi = _configuration.GetValue<bool>("PaymentSettings:EnableExternalApiIntegration", false);
                
                if (!enableExternalApi)
                {
                    _logger.LogInformation("External API integration disabled, returning mock success");
                    return new CreateOrderResponse
                    {
                        Status = true,
                        Message = "Order created successfully (mock mode)",
                        Code = "200"
                    };
                }

                // Convert to external API format
                var externalOrder = ConvertToExternalOrder(order, tickets);
                
                _logger.LogInformation("Creating external order for Order ID: {OrderId}", order.OrderId);
                
                // Call external API
                var response = await _bookingService.CreateRealOrderAsync(externalOrder);
                
                if (response.Status)
                {
                    _logger.LogInformation("External order created successfully for Order ID: {OrderId}", order.OrderId);
                }
                else
                {
                    _logger.LogWarning("External order creation failed for Order ID: {OrderId}. Message: {Message}", 
                        order.OrderId, response.Message);
                }
                
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating external order for Order ID: {OrderId}", order.OrderId);
                
                // Return success for internal processing even if external API fails
                return new CreateOrderResponse
                {
                    Status = true,
                    Message = "Order created internally (external API failed)",
                    Code = "200"
                };
            }
        }

        /// <summary>
        /// Convert internal order and tickets to external API format
        /// </summary>
        private ExternalOrderRequest ConvertToExternalOrder(Order order, List<Ticket> tickets)
        {
            var externalOrder = new ExternalOrderRequest
            {
                Booker = order.ContactName,
                ContactNo = order.ContactPhone ?? "",
                Email = order.ContactEmail ?? "",
                Buyer = order.ContactName,
                Taxcode = "", // Could be added to Order model if needed
                CompNm = "", // Could be added to Order model if needed
                CompAddress = "", // Could be added to Order model if needed
                TotalNumber = tickets.Count,
                TotalAmount = order.TotalAmount,
                BoatId = 1, // Default boat ID
                VoyageId = order.ExternalVoyageId ?? 1,
                ScheduleId = order.ExternalScheduleId ?? 1,
                PaidAmount = order.TotalAmount,
                RouteId = order.ExternalRouteId ?? 1,
                DepartDate = order.DepartureDate.ToString("yyyy-MM-dd"),
                Tickets = ConvertToExternalTickets(tickets)
            };

            return externalOrder;
        }

        /// <summary>
        /// Convert internal tickets to external API format
        /// </summary>
        private List<ExternalTicketRequest> ConvertToExternalTickets(List<Ticket> tickets)
        {
            var externalTickets = new List<ExternalTicketRequest>();
            
            for (int i = 0; i < tickets.Count; i++)
            {
                var ticket = tickets[i];
                
                var externalTicket = new ExternalTicketRequest
                {
                    IdNo = ticket.PassengerIdCard ?? "",
                    FullNm = ticket.PassengerName,
                    POB = "Việt Nam", // Default place of birth
                    YOB = "01/01/1990", // Default birth date if not provided
                    TicketTypeId = GetTicketTypeId(ticket.TicketType),
                    TicketClass = ticket.TicketClass ?? "ECO",
                    NationId = 1, // Vietnam
                    PhoneNo = ticket.PassengerPhone ?? "",
                    Email = "", // Could be added to Ticket model if needed
                    PositionId = ticket.ExternalSeatId ?? 1,
                    TicketPriceId = 1, // Could be stored in ticket if needed
                    PriceWithVAT = ticket.Price,
                    No = i + 1, // Sequential number
                    IsVIP = ticket.TicketClass?.ToUpper().Contains("VIP") == true,
                    Gender = GetGenderFromName(ticket.PassengerName) // Simple gender detection
                };
                
                externalTickets.Add(externalTicket);
            }
            
            return externalTickets;
        }

        /// <summary>
        /// Map ticket type string to external API ticket type ID
        /// </summary>
        private int GetTicketTypeId(string? ticketType)
        {
            return ticketType?.ToLower() switch
            {
                "adult" => 1,
                "child" => 2,
                "infant" => 3,
                "senior" => 4,
                _ => 1 // Default to adult
            };
        }

        /// <summary>
        /// Simple gender detection based on name (fallback method)
        /// </summary>
        private int GetGenderFromName(string name)
        {
            // Simple heuristic - in real implementation, you might want to ask user for gender
            var femaleIndicators = new[] { "thị", "nữ", "hoa", "lan", "mai", "linh", "anh" };
            var nameLower = name.ToLower();
            
            foreach (var indicator in femaleIndicators)
            {
                if (nameLower.Contains(indicator))
                {
                    return 1; // Female
                }
            }
            
            return 0; // Default to male
        }

        /// <summary>
        /// Get available routes from external API
        /// </summary>
        public async Task<List<RouteResult>> GetRoutesAsync()
        {
            try
            {
                var routes = await _bookingService.GetRoutesAsync();
                return routes ?? new List<RouteResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching routes from external API");
                return new List<RouteResult>();
            }
        }

        /// <summary>
        /// Search voyages using external API
        /// </summary>
        public async Task<List<VoyageResult>> SearchVoyagesAsync(int routeId, string date, int passengers)
        {
            try
            {
                var voyages = await _bookingService.SearchVoyageAsync(routeId, date, passengers);
                return voyages ?? new List<VoyageResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching voyages from external API");
                return new List<VoyageResult>();
            }
        }

        /// <summary>
        /// Get available seats for a voyage
        /// </summary>
        public async Task<List<SeatEmptyResult>> GetAvailableSeatsAsync(int voyageId, string date)
        {
            try
            {
                var seats = await _bookingService.GetSeatsAsync(voyageId, date);
                return seats ?? new List<SeatEmptyResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching seats from external API");
                return new List<SeatEmptyResult>();
            }
        }

        /// <summary>
        /// Get ticket prices for a route
        /// </summary>
        public async Task<List<TicketPriceResult>> GetTicketPricesAsync(int routeId, int boatTypeId, string date)
        {
            try
            {
                var prices = await _bookingService.GetPricesAsync(routeId, boatTypeId, date);
                return prices ?? new List<TicketPriceResult>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ticket prices from external API");
                return new List<TicketPriceResult>();
            }
        }
    }
}
