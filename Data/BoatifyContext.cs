using Boatify.Models;
using Microsoft.EntityFrameworkCore;

namespace Boatify.Data
{
    public class BoatifyContext : DbContext
    {
        public BoatifyContext(DbContextOptions<BoatifyContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<StaffRole> StaffRoles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        //public DbSet<ServiceProvider> ServiceProviders { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; }
        public DbSet<TicketPrice> TicketPrices { get; set; }
        //public DbSet<Route> Routes { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<BoatSeat> BoatSeats { get; set; }
        public DbSet<Voyage> Voyages { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<OrderOnline> OrderOnlines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoatSeat>()
                .HasIndex(bs => new { bs.BoatId, bs.SeatId }).IsUnique();

            modelBuilder.Entity<Voyage>()
                .HasIndex(v => new { v.RouteId, v.BoatId, v.DepartureTime, v.ArrivalTime }).IsUnique();
        }
    }
}
