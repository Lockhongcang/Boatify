using System.Globalization;
using Boatify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boatify.Controllers
{
    public class SeatController : Controller
    {
        private readonly BookingService _bookingService;

        public SeatController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> Map(int id, int scheduleId, int routeId, string departure, string destination, string date)
        {
            var layout = await _bookingService.GetBoatLayoutAsync(id, scheduleId);

            var voyages = await _bookingService.SearchVoyageAsync(routeId, date, 1);
            var voyage = voyages.FirstOrDefault(v => v.VoyageId == id);
            if (voyage == null)
            {
                TempData["Error"] = "Không tìm thấy chuyến đi.";
                return RedirectToAction("Index", "Home");
            }

            var prices = await _bookingService.GetPricesAsync(
                routeId,
                voyage.BoatTypeId,
                date
            );

            // Gắn giá vào từng ghế theo TicketClass
            foreach (var seat in layout)
            {
                var ticketClass = seat.TicketClass?.ToUpper() ?? "LUX";
                var matchedPrice = prices.FirstOrDefault(p =>
                    p.TicketClass?.ToUpper() == ticketClass &&
                    p.TicketTypeId == 1 // chỉ lấy giá người lớn
                );

                seat.TicketPriceWithVAT = matchedPrice?.PriceWithVAT ?? 0;
            }

            ViewBag.Prices = prices;
            ViewBag.Departure = departure;
            ViewBag.Destination = destination;
            ViewBag.DepartTime = voyage.DepartTime;
            ViewBag.Date = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            ViewBag.VoyageId = id;
            ViewBag.ScheduleId = scheduleId;
            ViewBag.RouteId = routeId;

            return View(layout);
        }
    }
}
