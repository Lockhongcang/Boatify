using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class TicketPrice
{
    public int TicketPriceId { get; set; }

    public int TicketTypeId { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();

    public virtual ICollection<TicketOffline> TicketOfflines { get; set; } = new List<TicketOffline>();

    public virtual ICollection<TicketOnline> TicketOnlines { get; set; } = new List<TicketOnline>();

    public virtual TicketType TicketType { get; set; } = null!;
}
