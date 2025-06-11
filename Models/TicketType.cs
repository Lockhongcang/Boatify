using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class TicketType
{
    public int TicketTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TicketPrice> TicketPrices { get; set; } = new List<TicketPrice>();
}
