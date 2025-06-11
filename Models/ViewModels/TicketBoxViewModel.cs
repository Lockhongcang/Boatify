namespace Boatify.Models.ViewModels
{
    public class TicketBoxViewModel
    {
        public List<string> Departures { get; set; } = new();
        public List<string> Destinations { get; set; } = new();
        public string SelectedDeparture { get; set; } = string.Empty;
        public string SelectedDestination { get; set; } = string.Empty;
        public DateTime? SelectedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
