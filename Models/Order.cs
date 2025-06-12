using System.ComponentModel.DataAnnotations;

namespace Boatify.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string OrderCode { get; set; } = string.Empty;
        
        [Required]
        public string Departure { get; set; } = string.Empty;
        
        [Required]
        public string Destination { get; set; } = string.Empty;
        
        [Required]
        public DateTime DepartureDate { get; set; }
        
        public string DepartureTime { get; set; } = string.Empty;
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Cancelled, Failed
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ExpiresAt { get; set; }
        
        // Contact Information
        public string ContactName { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        
        // External API References
        public int? ExternalVoyageId { get; set; }
        public int? ExternalScheduleId { get; set; }
        public int? ExternalRouteId { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
