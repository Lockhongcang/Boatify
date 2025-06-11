using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class TicketOnline
{
    public int TicketOnlineId { get; set; }

    public int OrderOnlineId { get; set; }

    public int TicketPriceId { get; set; }

    public int BoatSeatId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public virtual BoatSeat BoatSeat { get; set; } = null!;

    public virtual OrderOnline OrderOnline { get; set; } = null!;

    public virtual TicketPrice TicketPrice { get; set; } = null!;
}
