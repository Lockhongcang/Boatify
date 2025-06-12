using Boatify.Models.Dto;

namespace Boatify.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public BookingData? BookingData { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; } = new();
        public ContactInfo ContactInfo { get; set; } = new();
        public string SelectedPaymentMethod { get; set; } = string.Empty;
    }

    public class OrderConfirmationViewModel
    {
        public OrderSummary Order { get; set; } = new();
        public string QrCode { get; set; } = string.Empty;
        public bool ShowPaymentInstructions { get; set; }
        public string PaymentInstructions { get; set; } = string.Empty;
    }

    public class PaymentStatusViewModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public OrderSummary? Order { get; set; }
    }
}
