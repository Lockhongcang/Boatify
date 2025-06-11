using Boatify.Models.ViewModels;
using Boatify.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boatify.ViewComponents
{
    public class TicketBoxViewComponent : ViewComponent
    {
        private readonly BookingService _bookingService;

        public TicketBoxViewComponent(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IViewComponentResult> InvokeAsync(TicketBoxViewModel? model)
        {
            var routes = await _bookingService.GetRoutesAsync();

            var departures = routes
                .Select(r => r.Label.Split(" - ")[0].Trim())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var destinations = routes
                .Select(r => r.Label.Split(" - ")[1].Trim())
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            model ??= new TicketBoxViewModel();
            model.Departures = departures;
            model.Destinations = destinations;

            return View(model);
        }
    }
}
