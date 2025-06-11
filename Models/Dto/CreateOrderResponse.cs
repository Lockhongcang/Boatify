namespace Boatify.Models.Dto
{
    public class CreateOrderResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public OrderData Data { get; set; }
    }

    public class OrderData
    {
        public string BookingCode { get; set; }
        public double TotalAmt { get; set; }
    }
}
