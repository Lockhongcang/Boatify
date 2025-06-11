using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int VoyageId { get; set; }

    public DateOnly DepartureDate { get; set; }

    public virtual ICollection<OrderOffline> OrderOfflines { get; set; } = new List<OrderOffline>();

    public virtual ICollection<OrderOnline> OrderOnlines { get; set; } = new List<OrderOnline>();

    public virtual Voyage Voyage { get; set; } = null!;
}
