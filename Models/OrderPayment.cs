using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class OrderPayment
{
    public int OrderPaymentId { get; set; }

    public int? OrderOnlineId { get; set; }

    public int? OrderOfflineId { get; set; }

    public string Provider { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public string? CallbackData { get; set; }

    public DateTime PaidDate { get; set; }

    public virtual OrderOffline? OrderOffline { get; set; }

    public virtual OrderOnline? OrderOnline { get; set; }
}
