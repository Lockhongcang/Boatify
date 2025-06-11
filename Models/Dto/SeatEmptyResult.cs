namespace Boatify.Models.Dto
{
    public class SeatEmptyResult
    {
        public int SeatId { get; set; }
        public int PositionId { get; set; }
        public string SeatNm { get; set; }
        public string TicketClass { get; set; }
        public bool IsVIP { get; set; }
    }
}
