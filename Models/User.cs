using System.ComponentModel.DataAnnotations;

namespace Boatify.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        public string FullName { get; set; } = string.Empty;
        
        public string? Phone { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
