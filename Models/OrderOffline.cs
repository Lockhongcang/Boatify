using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class OrderOffline
{
    public int OrderOfflineId { get; set; }

    public int CustomerId { get; set; }

    public int ScheduleId { get; set; }

    public int VoyageId { get; set; }

    public int StaffId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<OrderPayment> OrderPayments { get; set; } = new List<OrderPayment>();

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;

    public virtual ICollection<TicketOffline> TicketOfflines { get; set; } = new List<TicketOffline>();

    public virtual Voyage Voyage { get; set; } = null!;
}
