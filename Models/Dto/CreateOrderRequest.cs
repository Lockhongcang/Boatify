namespace Boatify.Models.Dto
{
    public class CreateOrderRequest
    {
        public int RouteId { get; set; }
        public int VoyageId { get; set; }
        public int ScheduleId { get; set; }
        public string DepartDate { get; set; } = string.Empty;
        public List<int> SeatIds { get; set; } = new();

        public string ContactName { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;

        public List<PassengerDto> PassengerList { get; set; } = new();
    }

    public class PassengerDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int SeatId { get; set; }
    }
}
