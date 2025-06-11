using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class ServiceProvider
{
    public int ServiceProviderId { get; set; }

    public string Name { get; set; } = null!;

    public string? ContactInfo { get; set; }

    public string ApiEndpoint { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Route> Routes { get; set; } = new List<Route>();
}
