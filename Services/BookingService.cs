using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Boatify.Models.Dto;

namespace Boatify.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://192.168.1.125:7878";
        private readonly string _username = "VNPay.APP";
        private readonly string _password = "ji@u$yzxu@cd92e";

        private readonly ILogger<BookingService> _logger;

        public BookingService(HttpClient httpClient, ILogger<BookingService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        private HttpRequestMessage CreateRequest(HttpMethod method, string url)
        {
            var request = new HttpRequestMessage(method, url);

            // Thêm Basic Auth
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Header API Key
            request.Headers.Add("HVC.APIKey", "*HVC#*!2e2A");

            return request;
        }

        private async Task<T?> SendRequestAsync<T>(HttpRequestMessage request)
        {
            try
            {
                _logger.LogInformation("➡️ URL: {Url}", request.RequestUri);
                _logger.LogInformation("➡️ Method: {Method}", request.Method);
                foreach (var h in request.Headers)
                    _logger.LogInformation("➡️ Header: {Key}: {Value}", h.Key, string.Join(", ", h.Value));

                if (request.Content != null)
                {
                    var content = await request.Content.ReadAsStringAsync();
                    _logger.LogInformation("➡️ Body: {Body}", content);
                }
                else
                {
                    _logger.LogInformation("➡️ Body: (none)");
                }

                var response = await _httpClient.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("❌ API failed ({StatusCode}): {Body}", response.StatusCode, responseBody);
                    return GetMockData<T>();
                }

                _logger.LogWarning("⬅️ Status: {Status}", response.StatusCode);
                _logger.LogWarning("⬅️ Response: {Body}", responseBody);

                return JsonSerializer.Deserialize<T>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ External API error, returning mock data");
                return GetMockData<T>();
            }
        }

        private T? GetMockData<T>()
        {
            _logger.LogWarning("🔄 Returning mock data for type: {Type}", typeof(T).Name);

            if (typeof(T) == typeof(List<RouteResult>))
            {
                var mockRoutes = new List<RouteResult>
                {
                    new RouteResult { RouteId = 1, Label = "Hà Nội - Hải Phòng" },
                    new RouteResult { RouteId = 2, Label = "TP.HCM - Vũng Tàu" }
                };
                return (T)(object)mockRoutes;
            }

            if (typeof(T) == typeof(List<VoyageResult>))
            {
                var mockVoyages = new List<VoyageResult>
                {
                    new VoyageResult
                    {
                        VoyageId = 1,
                        BoatTypeNm = "Tàu cao tốc",
                        RouteNm = "Hà Nội - Hải Phòng",
                        BoatNm = "Boat 001",
                        Time = "2 giờ",
                        DepartTime = "08:00",
                        Harbor = "Bến tàu Hà Nội"
                    }
                };
                return (T)(object)mockVoyages;
            }

            if (typeof(T) == typeof(List<SeatEmptyResult>))
            {
                var mockSeats = new List<SeatEmptyResult>
                {
                    new SeatEmptyResult { SeatId = 1, SeatNm = "A01", TicketClass = "Economy" },
                    new SeatEmptyResult { SeatId = 2, SeatNm = "A02", TicketClass = "Economy" },
                    new SeatEmptyResult { SeatId = 3, SeatNm = "A03", TicketClass = "Economy" },
                    new SeatEmptyResult { SeatId = 4, SeatNm = "B01", TicketClass = "Business" }
                };
                return (T)(object)mockSeats;
            }

            if (typeof(T) == typeof(CreateOrderResponse))
            {
                var mockResponse = new CreateOrderResponse
                {
                    Status = true,
                    Message = "Mock order created successfully",
                    Code = "200"
                };
                return (T)(object)mockResponse;
            }

            return default(T);
        }

        public async Task<List<RouteResult>> GetRoutesAsync()
        {
            var url = $"{_baseUrl}/Route/GetRoutes";
            var request = CreateRequest(HttpMethod.Get, url);
            return await SendRequestAsync<List<RouteResult>>(request);
        }

        public async Task<List<VoyageResult>> SearchVoyageAsync(int routeId, string date, int pax)
        {
            var url = $"{_baseUrl}/OnlineSearchVoyageResult/SearchVoyage?RouteId={routeId}&DepartDate={date}&NoOfPassenger={pax}";
            var request = CreateRequest(HttpMethod.Get, url);
            return await SendRequestAsync<List<VoyageResult>>(request);
        }

        public async Task<List<SeatEmptyResult>> GetSeatsAsync(int voyageId, string date)
        {
            var url = $"{_baseUrl}/Voyage/GetSeatsEmpty?voyageId={voyageId}&departDate={date}";
            var request = CreateRequest(HttpMethod.Get, url);
            return await SendRequestAsync<List<SeatEmptyResult>>(request);
        }

        public async Task<List<TicketPriceResult>> GetPricesAsync(int routeId, int boatTypeId, string date)
        {
            var url = $"{_baseUrl}/TicketPrice/GetTicketPriceByRouteId?RouteId={routeId}&BoatTypeId={boatTypeId}&DepartDate={date}";
            var request = CreateRequest(HttpMethod.Get, url);
            return await SendRequestAsync<List<TicketPriceResult>>(request);
        }

        public async Task<List<BoatLayoutResult>> GetBoatLayoutAsync(int voyageId, int scheduleId)
        {
            var url = $"{_baseUrl}/Voyage/GetBoatLayout?voyageId={voyageId}&scheduleId={scheduleId}";
            var request = CreateRequest(HttpMethod.Get, url);
            return await SendRequestAsync<List<BoatLayoutResult>>(request);
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(object orderBody)
        {
            var url = $"{_baseUrl}/OrderOnline/CreateOrderOnline";
            var request = CreateRequest(HttpMethod.Post, url);
            var jsonBody = JsonSerializer.Serialize(orderBody);
            Console.WriteLine("➡️ JSON BODY:\n" + jsonBody);
            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return await SendRequestAsync<CreateOrderResponse>(request);
        }

        /// <summary>
        /// Create order using the real API format from documentation
        /// </summary>
        public async Task<CreateOrderResponse> CreateRealOrderAsync(ExternalOrderRequest orderRequest)
        {
            var url = $"{_baseUrl}/OrderOnline/CreateOrderOnline";
            var request = CreateRequest(HttpMethod.Post, url);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var jsonBody = JsonSerializer.Serialize(orderRequest, jsonOptions);
            _logger.LogInformation("➡️ Creating Real Order JSON:\n{JsonBody}", jsonBody);

            request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return await SendRequestAsync<CreateOrderResponse>(request);
        }
        public async Task<string> GetRawSearchVoyageResponseAsync(int routeId, string date)
        {
            var url = $"{_baseUrl}/OnlineSearchVoyageResult/SearchVoyage?RouteId={routeId}&DepartDate={date}&NoOfPassenger=1";
            var request = CreateRequest(HttpMethod.Get, url);

            _logger.LogInformation("📡 RAW SearchVoyage URL: {Url}", url);

            var response = await _httpClient.SendAsync(request);
            var raw = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("📥 RAW Response: {Body}", raw);

            return raw;
        }

    }
}
