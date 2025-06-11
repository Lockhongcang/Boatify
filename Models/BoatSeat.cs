using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class BoatSeat
{
    public int BoatSeatId { get; set; }

    public int BoatId { get; set; }

    public int SeatId { get; set; }

    public bool IsBooked { get; set; }

    public virtual Boat Boat { get; set; } = null!;

    public virtual Seat Seat { get; set; } = null!;

    public virtual ICollection<TicketOffline> TicketOfflines { get; set; } = new List<TicketOffline>();

    public virtual ICollection<TicketOnline> TicketOnlines { get; set; } = new List<TicketOnline>();
}
