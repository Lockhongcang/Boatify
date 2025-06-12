using System.ComponentModel.DataAnnotations;

namespace Boatify.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public string SeatCode { get; set; } = string.Empty;
        
        [Required]
        public string PassengerName { get; set; } = string.Empty;
        
        public string PassengerPhone { get; set; } = string.Empty;
        
        public string PassengerIdCard { get; set; } = string.Empty;
        
        public DateTime? PassengerDateOfBirth { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        public string TicketType { get; set; } = "Adult"; // Adult, Child, Senior
        
        public string TicketClass { get; set; } = "Economy"; // Economy, Business, VIP
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // External API References
        public int? ExternalSeatId { get; set; }
        public int? ExternalTicketPriceId { get; set; }
        
        // Navigation property
        public virtual Order Order { get; set; } = null!;
    }
}
