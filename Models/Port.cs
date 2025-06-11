using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Port
{
    public int PortId { get; set; }

    public string PortName { get; set; } = null!;

    public string? Location { get; set; }

    public virtual ICollection<Route> RouteArrivalPorts { get; set; } = new List<Route>();

    public virtual ICollection<Route> RouteDeparturePorts { get; set; } = new List<Route>();
}
