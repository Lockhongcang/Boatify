namespace Boatify.Models.Dto
{
    public class BoatLayoutResult
    {
        public int PositionId { get; set; }
        public int SeatId { get; set; }
        public string TicketClass { get; set; }
        public string SeatNm { get; set; }
        public bool IsSeat { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public int IsUpStair { get; set; }
        public bool IsBooked { get; set; }
        public bool IsPublished { get; set; }
        public string BookedInfo { get; set; }
        public bool IsVIP { get; set; }
        public bool IsHeld { get; set; }
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }
        public bool IsRotate { get; set; }
        public double TicketPriceWithVAT { get; set; } = 0;

    }
}
