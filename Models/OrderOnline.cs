using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class OrderOnline
{
    public int OrderOnlineId { get; set; }

    public int CustomerId { get; set; }

    public int ScheduleId { get; set; }

    public int VoyageId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime ExpiresAt { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual ICollection<TicketOnline> TicketOnlines { get; set; } = new List<TicketOnline>();

    public virtual Voyage Voyage { get; set; } = null!;
}
