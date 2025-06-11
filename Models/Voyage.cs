using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Voyage
{
    public int VoyageId { get; set; }

    public int RouteId { get; set; }

    public int BoatId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public virtual Boat Boat { get; set; } = null!;

    public virtual ICollection<OrderOffline> OrderOfflines { get; set; } = new List<OrderOffline>();

    public virtual ICollection<OrderOnline> OrderOnlines { get; set; } = new List<OrderOnline>();

    public virtual Route Route { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
