using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class TicketOffline
{
    public int TicketOfflineId { get; set; }

    public int OrderOfflineId { get; set; }

    public int TicketPriceId { get; set; }

    public int BoatSeatId { get; set; }

    public virtual BoatSeat BoatSeat { get; set; } = null!;

    public virtual OrderOffline OrderOffline { get; set; } = null!;

    public virtual TicketPrice TicketPrice { get; set; } = null!;
}
