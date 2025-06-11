namespace Boatify.Models.Dto
{
    public class RouteResult
    {
        public int RouteId { get; set; }
        public string Label { get; set; } = string.Empty;

        public string DeparturePortName => Label.Split(" - ").FirstOrDefault() ?? "";
        public string ArrivalPortName => Label.Split(" - ").Skip(1).FirstOrDefault() ?? "";
    }
}
