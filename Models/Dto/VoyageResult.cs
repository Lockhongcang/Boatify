namespace Boatify.Models.Dto
{
    public class VoyageResult
    {
        public int BoatTypeId { get; set; }
        public string BoatTypeNm { get; set; }
        public int VoyageId { get; set; }
        public int ScheduleId { get; set; }
        public int RouteId { get; set; }
        public string RouteNm { get; set; }
        public int BoatId { get; set; }
        public string BoatNm { get; set; }
        public string Time { get; set; }
        public bool Disabled { get; set; }
        public int NoOfRemain { get; set; }
        public string DepartTime { get; set; }
        public string Harbor { get; set; }
    }

}
