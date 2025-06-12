namespace Boatify.Models.Dto
{
    public class CreateOrderResponse
    {
        public bool Status { get; set; }
        public bool Success => Status; // Alias for backward compatibility
        public string Message { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public OrderData? Data { get; set; }
    }

    public class OrderData
    {
        public string BookingCode { get; set; } = string.Empty;
        public double TotalAmt { get; set; }
    }
}
