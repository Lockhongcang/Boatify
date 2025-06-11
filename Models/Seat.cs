using System;
using System.Collections.Generic;

namespace Boatify.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public string SeatName { get; set; } = null!;

    public virtual ICollection<BoatSeat> BoatSeats { get; set; } = new List<BoatSeat>();
}
