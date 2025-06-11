using Boatify.Models.Dto;
using Boatify.Models.ViewModels;
using Boatify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boatify.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookingService _bookingService;

        public HomeController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index()
        {
            var allRoutes = await _bookingService.GetRoutesAsync();

            var mapped = allRoutes
                .Where(r => !string.IsNullOrWhiteSpace(r.Label) && r.Label.Contains(" - "))
                .Select(r =>
                {
                    var parts = r.Label.Split(" - ");
                    return new
                    {
                        Departure = parts[0].Trim(),
                        Destination = parts[1].Trim()
                    };
                })
                .ToList();

            var viewModel = new BookingViewModel
            {
                TicketBox = new TicketBoxViewModel
                {
                    Departures = mapped.Select(x => x.Departure).Distinct().ToList(),
                    Destinations = mapped.Select(x => x.Destination).Distinct().ToList()
                },
                Voyages = new List<VoyageResult>() // ban đầu để trống
            };

            return View(viewModel);
        }
    }
}
