using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Boat
{
    public int BoatId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public virtual ICollection<BoatSeat> BoatSeats { get; set; } = new List<BoatSeat>();

    public virtual ICollection<Voyage> Voyages { get; set; } = new List<Voyage>();
}
