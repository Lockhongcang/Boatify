using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class StaffRole
{
    public int StaffRoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
