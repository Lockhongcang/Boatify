using Boatify.Models.Dto;

namespace Boatify.Models.ViewModels
{
    public class BookingViewModel
    {
        public TicketBoxViewModel TicketBox { get; set; }
        public List<VoyageResult> Voyages { get; set; } = new();
    }
}
