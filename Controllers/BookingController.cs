using Boatify.Models.ViewModels;
using Boatify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boatify.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string departure, string destination, DateTime? date, int ticketCount = 1)
        {
            Console.WriteLine("➡ Vào BookingController.Index()");
            Console.WriteLine($"[PARAMS] departure: {departure}, destination: {destination}, date: {date}, ticketCount: {ticketCount}");

            if (string.IsNullOrEmpty(departure) || string.IsNullOrEmpty(destination) || !date.HasValue)
            {
                Console.WriteLine("❌ Thiếu tham số");
                TempData["Error"] = "Vui lòng chọn điểm đi, điểm đến và ngày đi trước khi tìm chuyến.";
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("📡 Đang gọi GetRoutesAsync...");
            var allRoutes = await _bookingService.GetRoutesAsync();
            Console.WriteLine($"✅ Tổng tuyến: {allRoutes.Count}");

            var selectedRoute = allRoutes
    .Where(r => !string.IsNullOrEmpty(r.Label))
    .Select(r => new
    {
        r.RouteId,
        Departure = r.Label.Split(" - ")[0].Trim(),
        Destination = r.Label.Split(" - ")[1].Trim(),
        r.Label
    })
    .FirstOrDefault(r =>
        (r.Departure.Equals(departure, StringComparison.OrdinalIgnoreCase) &&
         r.Destination.Equals(destination, StringComparison.OrdinalIgnoreCase)) ||
        (r.Departure.Equals(destination, StringComparison.OrdinalIgnoreCase) &&
         r.Destination.Equals(departure, StringComparison.OrdinalIgnoreCase))
    );

            if (selectedRoute == null)
            {
                Console.WriteLine("❌ Không tìm thấy tuyến phù hợp.");
                TempData["Error"] = "Không tìm thấy tuyến phù hợp.";
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine($"✅ RouteId: {selectedRoute.RouteId}");

            // 🔥 In raw response từ SearchVoyage
            var rawJson = await _bookingService.GetRawSearchVoyageResponseAsync(
                selectedRoute.RouteId,
                date.Value.ToString("yyyy-MM-dd")
            );
            Console.WriteLine("🔥 Raw Voyage Response:\n" + rawJson);

            Console.WriteLine("📡 Gọi SearchVoyageAsync...");
            var voyages = await _bookingService.SearchVoyageAsync(
                selectedRoute.RouteId,
                date.Value.ToString("yyyy-MM-dd"),
                ticketCount
            );
            Console.WriteLine($"✅ Tổng chuyến: {voyages.Count}");

            //var firstVoyage = voyages.First();

            //return RedirectToAction("Map", "Seat", new
            //{
            //    id = firstVoyage.VoyageId,
            //    scheduleId = firstVoyage.ScheduleId,
            //    routeId = selectedRoute.RouteId,
            //    departure,
            //    destination,
            //    date = date.Value.ToString("yyyy-MM-dd")
            //});

            var priceDict = new Dictionary<int, double>();
            foreach (var voyage in voyages)
            {
                Console.WriteLine($"🔍 Gọi GetPricesAsync cho VoyageId: {voyage.VoyageId}, BoatTypeId: {voyage.BoatTypeId}");
                var prices = await _bookingService.GetPricesAsync(
                    selectedRoute.RouteId,
                    voyage.BoatTypeId,
                    date.Value.ToString("yyyy-MM-dd")
                );

                var price = prices.FirstOrDefault()?.PriceWithVAT ?? 0;
                priceDict[voyage.VoyageId] = price;
                Console.WriteLine($"✅ Giá vé: {price}");
            }

            ViewBag.PriceDict = priceDict;

            var ticketBox = new TicketBoxViewModel
            {
                Departures = allRoutes.Select(r => r.Label.Split(" - ")[0].Trim()).Distinct().ToList(),
                Destinations = allRoutes.Select(r => r.Label.Split(" - ")[1].Trim()).Distinct().ToList(),
                SelectedDeparture = departure,
                SelectedDestination = destination,
                SelectedDate = date
            };

            var viewModel = new BookingViewModel
            {
                TicketBox = ticketBox,
                Voyages = voyages
            };

            Console.WriteLine("✅ Render View xong.");
            return View(viewModel);
        }
    }
}
