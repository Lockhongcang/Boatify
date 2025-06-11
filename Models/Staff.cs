using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public int AccountId { get; set; }

    public int StaffRoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<OrderOffline> OrderOfflines { get; set; } = new List<OrderOffline>();

    public virtual StaffRole StaffRole { get; set; } = null!;
}
