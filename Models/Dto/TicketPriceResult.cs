namespace Boatify.Models.Dto
{
    public class TicketPriceResult
    {
        public int TicketPriceId { get; set; }
        public int RouteId { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketClass { get; set; }
        public string TicketTypeLabel { get; set; }
        public double PriceWithVAT { get; set; }
    }
}
