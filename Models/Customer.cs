using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int AccountId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<OrderOffline> OrderOfflines { get; set; } = new List<OrderOffline>();

    public virtual ICollection<OrderOnline> OrderOnlines { get; set; } = new List<OrderOnline>();
}
