using System.ComponentModel.DataAnnotations;

namespace Boatify.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public string PaymentMethod { get; set; } = string.Empty; // VNPay, MoMo, Banking, Cash
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Success, Failed, Cancelled
        
        public string? TransactionId { get; set; }
        
        public string? PaymentGatewayResponse { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? PaidAt { get; set; }
        
        public string? FailureReason { get; set; }
        
        // Payment Gateway Specific Data
        public string? GatewayTransactionId { get; set; }
        public string? GatewayOrderId { get; set; }
        public string? CallbackData { get; set; }
        
        // Navigation property
        public virtual Order Order { get; set; } = null!;
    }
}
