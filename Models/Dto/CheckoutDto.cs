namespace Boatify.Models.Dto
{
    public class CheckoutRequest
    {
        public BookingData BookingData { get; set; } = new();
        public string PaymentMethod { get; set; } = string.Empty;
        public ContactInfo ContactInfo { get; set; } = new();
    }

    public class BookingData
    {
        public int VoyageId { get; set; }
        public int ScheduleId { get; set; }
        public int RouteId { get; set; }
        public string DepartDate { get; set; } = string.Empty;
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public List<SelectedSeat> SelectedSeats { get; set; } = new();
        public double TotalAmount { get; set; }
    }

    public class SelectedSeat
    {
        public int SeatId { get; set; }
        public string SeatCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
        public string Cccd { get; set; } = string.Empty;
        public double Price { get; set; }
    }

    public class ContactInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public CustomerInfo CustomerInfo { get; set; } = new();
        public string OrderInfo { get; set; } = string.Empty;
    }

    public class CustomerInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string PaymentUrl { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class PaymentMethod
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class MoMoResponse
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string PayUrl { get; set; } = string.Empty;
        public string Deeplink { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
    }



    public class PaymentCallbackRequest
    {
        public string Provider { get; set; } = string.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new();
    }

    public class OrderSummary
    {
        public int OrderId { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Departure { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public List<TicketInfo> Tickets { get; set; } = new();
        public PaymentInfo Payment { get; set; } = new();
    }

    public class TicketInfo
    {
        public string SeatCode { get; set; } = string.Empty;
        public string PassengerName { get; set; } = string.Empty;
        public string PassengerPhone { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }

    public class PaymentInfo
    {
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? PaidDate { get; set; }
        public decimal Amount { get; set; }
    }
}
