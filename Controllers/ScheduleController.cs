// Controllers/ScheduleController.cs
using Boatify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boatify.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly BookingService _bookingService;

        public ScheduleController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // GET /Schedule
        public async Task<IActionResult> Index(string date)
        {
            // nếu không truyền date thì lấy ngày hôm nay
            var dt = string.IsNullOrEmpty(date)
              ? DateTime.Today.ToString("yyyy-MM-dd")
              : date;
            ViewBag.Date = dt;
            var routes = await _bookingService.GetRoutesAsync();
            return View(routes);
        }

        // GET /Schedule/Voyages?routeId=xx&date=yyyy-MM-dd&pax=1
        [HttpGet]
        public async Task<PartialViewResult> Voyages(int routeId, string date, int pax = 1)
        {
            // date đã validate ở Index, nếu không thì default
            var dt = string.IsNullOrEmpty(date)
              ? DateTime.Today.ToString("yyyy-MM-dd")
              : date;
            var list = await _bookingService.SearchVoyageAsync(routeId, dt, pax);
            return PartialView("_VoyageList", list);
        }
    }
}
