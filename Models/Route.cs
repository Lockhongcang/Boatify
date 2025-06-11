using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Route
{
    public int RouteId { get; set; }

    public int DeparturePortId { get; set; }

    public int ArrivalPortId { get; set; }

    public int TicketPriceId { get; set; }

    public int DurationMinutes { get; set; }

    public int ServiceProviderId { get; set; }

    public virtual Port ArrivalPort { get; set; } = null!;

    public virtual Port DeparturePort { get; set; } = null!;

    public virtual ServiceProvider ServiceProvider { get; set; } = null!;

    public virtual TicketPrice TicketPrice { get; set; } = null!;

    public virtual ICollection<Voyage> Voyages { get; set; } = new List<Voyage>();
}
