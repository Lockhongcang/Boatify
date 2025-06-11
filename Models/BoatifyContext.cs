using Microsoft.EntityFrameworkCore;

namespace Boatify.Models;

public partial class BoatifyContext : DbContext
{
    public BoatifyContext()
    {
    }

    public BoatifyContext(DbContextOptions<BoatifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Boat> Boats { get; set; }

    public virtual DbSet<BoatSeat> BoatSeats { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<OrderOffline> OrderOfflines { get; set; }

    public virtual DbSet<OrderOnline> OrderOnlines { get; set; }

    public virtual DbSet<OrderPayment> OrderPayments { get; set; }

    public virtual DbSet<Port> Ports { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<ServiceProvider> ServiceProviders { get; set; }

    public virtual DbSet<Staff> Staffs { get; set; }

    public virtual DbSet<StaffRole> StaffRoles { get; set; }

    public virtual DbSet<TicketOffline> TicketOfflines { get; set; }

    public virtual DbSet<TicketOnline> TicketOnlines { get; set; }

    public virtual DbSet<TicketPrice> TicketPrices { get; set; }

    public virtual DbSet<TicketType> TicketTypes { get; set; }

    public virtual DbSet<Voyage> Voyages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=TLOX.;Database=db_BoatifyBRB;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA5862976377B");

            entity.HasIndex(e => e.Email, "UQ__Accounts__A9D1053411C876E2").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.PasswordHash).HasMaxLength(200);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Boat>(entity =>
        {
            entity.HasKey(e => e.BoatId).HasName("PK__Boats__148829BC49099600");

            entity.Property(e => e.BoatId).HasColumnName("BoatID");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<BoatSeat>(entity =>
        {
            entity.HasKey(e => e.BoatSeatId).HasName("PK__BoatSeat__C945E54E96F148FF");

            entity.HasIndex(e => new { e.BoatId, e.SeatId }, "UQ_BoatSeat").IsUnique();

            entity.Property(e => e.BoatSeatId).HasColumnName("BoatSeatID");
            entity.Property(e => e.BoatId).HasColumnName("BoatID");
            entity.Property(e => e.SeatId).HasColumnName("SeatID");

            entity.HasOne(d => d.Boat).WithMany(p => p.BoatSeats)
                .HasForeignKey(d => d.BoatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BoatSeats_Boat");

            entity.HasOne(d => d.Seat).WithMany(p => p.BoatSeats)
                .HasForeignKey(d => d.SeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BoatSeats_Seats");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8353EB4B3");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Accounts");
        });

        modelBuilder.Entity<OrderOffline>(entity =>
        {
            entity.HasKey(e => e.OrderOfflineId).HasName("PK__OrderOff__E90C97C04F440E18");

            entity.Property(e => e.OrderOfflineId).HasColumnName("OrderOfflineID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Confirmed");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoyageId).HasColumnName("VoyageID");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderOfflines)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOfflines_Customers");

            entity.HasOne(d => d.Schedule).WithMany(p => p.OrderOfflines)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOfflines_Schedules");

            entity.HasOne(d => d.Staff).WithMany(p => p.OrderOfflines)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOfflines_Staff");

            entity.HasOne(d => d.Voyage).WithMany(p => p.OrderOfflines)
                .HasForeignKey(d => d.VoyageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOfflines_Voyages");
        });

        modelBuilder.Entity<OrderOnline>(entity =>
        {
            entity.HasKey(e => e.OrderOnlineId).HasName("PK__OrderOnl__9AD7C2E606EB8E25");

            entity.Property(e => e.OrderOnlineId).HasColumnName("OrderOnlineID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.ExpiresAt).HasDefaultValueSql("(dateadd(day,(7),getutcdate()))");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VoyageId).HasColumnName("VoyageID");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderOnlines)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOnlines_Customers");

            entity.HasOne(d => d.Schedule).WithMany(p => p.OrderOnlines)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOnlines_Schedules");

            entity.HasOne(d => d.Voyage).WithMany(p => p.OrderOnlines)
                .HasForeignKey(d => d.VoyageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderOnlines_Voyages");
        });

        modelBuilder.Entity<OrderPayment>(entity =>
        {
            entity.HasKey(e => e.OrderPaymentId).HasName("PK__OrderPay__C33CDF3135C8B8E4");

            entity.Property(e => e.OrderPaymentId).HasColumnName("OrderPaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderOfflineId).HasColumnName("OrderOfflineID");
            entity.Property(e => e.OrderOnlineId).HasColumnName("OrderOnlineID");
            entity.Property(e => e.PaidDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Provider).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.OrderOffline).WithMany(p => p.OrderPayments)
                .HasForeignKey(d => d.OrderOfflineId)
                .HasConstraintName("FK_OrderPayments_OrderOffline");

            entity.HasOne(d => d.OrderOnline).WithMany(p => p.OrderPayments)
                .HasForeignKey(d => d.OrderOnlineId)
                .HasConstraintName("FK_OrderPayments_OrderOnline");
        });

        modelBuilder.Entity<Port>(entity =>
        {
            entity.HasKey(e => e.PortId).HasName("PK__Ports__D859BFAFC3FF8190");

            entity.Property(e => e.PortId).HasColumnName("PortID");
            entity.Property(e => e.Location).HasMaxLength(300);
            entity.Property(e => e.PortName).HasMaxLength(200);
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.RouteId).HasName("PK__Routes__80979AAD1DAA45B8");

            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.ArrivalPortId).HasColumnName("ArrivalPortID");
            entity.Property(e => e.DeparturePortId).HasColumnName("DeparturePortID");
            entity.Property(e => e.ServiceProviderId).HasColumnName("ServiceProviderID");
            entity.Property(e => e.TicketPriceId).HasColumnName("TicketPriceID");

            entity.HasOne(d => d.ArrivalPort).WithMany(p => p.RouteArrivalPorts)
                .HasForeignKey(d => d.ArrivalPortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_ArrPort");

            entity.HasOne(d => d.DeparturePort).WithMany(p => p.RouteDeparturePorts)
                .HasForeignKey(d => d.DeparturePortId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_DepPort");

            entity.HasOne(d => d.ServiceProvider).WithMany(p => p.Routes)
                .HasForeignKey(d => d.ServiceProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_SP");

            entity.HasOne(d => d.TicketPrice).WithMany(p => p.Routes)
                .HasForeignKey(d => d.TicketPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Price");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B690E39C186");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.VoyageId).HasColumnName("VoyageID");

            entity.HasOne(d => d.Voyage).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.VoyageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedules_Voyage");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__311713D30C1865D1");

            entity.Property(e => e.SeatId).HasColumnName("SeatID");
            entity.Property(e => e.SeatName).HasMaxLength(20);
        });

        modelBuilder.Entity<ServiceProvider>(entity =>
        {
            entity.HasKey(e => e.ServiceProviderId).HasName("PK__ServiceP__5AD42B73792CED5C");

            entity.Property(e => e.ServiceProviderId).HasColumnName("ServiceProviderID");
            entity.Property(e => e.ApiEndpoint).HasMaxLength(500);
            entity.Property(e => e.ApiKey).HasMaxLength(200);
            entity.Property(e => e.ContactInfo).HasMaxLength(300);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.StaffId).HasName("PK__Staffs__96D4AAF75267161B");

            entity.Property(e => e.StaffId).HasColumnName("StaffID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.StaffRoleId).HasColumnName("StaffRoleID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Account).WithMany(p => p.Staff)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staffs_Accounts");

            entity.HasOne(d => d.StaffRole).WithMany(p => p.Staff)
                .HasForeignKey(d => d.StaffRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Staffs_StaffRoles");
        });

        modelBuilder.Entity<StaffRole>(entity =>
        {
            entity.HasKey(e => e.StaffRoleId).HasName("PK__StaffRol__10792D71CC4C2383");

            entity.Property(e => e.StaffRoleId).HasColumnName("StaffRoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<TicketOffline>(entity =>
        {
            entity.HasKey(e => e.TicketOfflineId).HasName("PK__TicketOf__A912F1B8C1D76C2B");

            entity.Property(e => e.TicketOfflineId).HasColumnName("TicketOfflineID");
            entity.Property(e => e.BoatSeatId).HasColumnName("BoatSeatID");
            entity.Property(e => e.OrderOfflineId).HasColumnName("OrderOfflineID");
            entity.Property(e => e.TicketPriceId).HasColumnName("TicketPriceID");

            entity.HasOne(d => d.BoatSeat).WithMany(p => p.TicketOfflines)
                .HasForeignKey(d => d.BoatSeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOfflines_BoatSeat");

            entity.HasOne(d => d.OrderOffline).WithMany(p => p.TicketOfflines)
                .HasForeignKey(d => d.OrderOfflineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOfflines_OrderOffline");

            entity.HasOne(d => d.TicketPrice).WithMany(p => p.TicketOfflines)
                .HasForeignKey(d => d.TicketPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOfflines_Price");
        });

        modelBuilder.Entity<TicketOnline>(entity =>
        {
            entity.HasKey(e => e.TicketOnlineId).HasName("PK__TicketOn__CF920E9B1AA227C2");

            entity.Property(e => e.TicketOnlineId).HasColumnName("TicketOnlineID");
            entity.Property(e => e.BoatSeatId).HasColumnName("BoatSeatID");
            entity.Property(e => e.ExpiresAt).HasDefaultValueSql("(dateadd(day,(7),getutcdate()))");
            entity.Property(e => e.OrderOnlineId).HasColumnName("OrderOnlineID");
            entity.Property(e => e.TicketPriceId).HasColumnName("TicketPriceID");

            entity.HasOne(d => d.BoatSeat).WithMany(p => p.TicketOnlines)
                .HasForeignKey(d => d.BoatSeatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOnlines_BoatSeat");

            entity.HasOne(d => d.OrderOnline).WithMany(p => p.TicketOnlines)
                .HasForeignKey(d => d.OrderOnlineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOnlines_OrderOnline");

            entity.HasOne(d => d.TicketPrice).WithMany(p => p.TicketOnlines)
                .HasForeignKey(d => d.TicketPriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketOnlines_Price");
        });

        modelBuilder.Entity<TicketPrice>(entity =>
        {
            entity.HasKey(e => e.TicketPriceId).HasName("PK__TicketPr__BE7DED9C7BE3EEB0");

            entity.Property(e => e.TicketPriceId).HasColumnName("TicketPriceID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TicketTypeId).HasColumnName("TicketTypeID");

            entity.HasOne(d => d.TicketType).WithMany(p => p.TicketPrices)
                .HasForeignKey(d => d.TicketTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TicketPrices_TicketTypes");
        });

        modelBuilder.Entity<TicketType>(entity =>
        {
            entity.HasKey(e => e.TicketTypeId).HasName("PK__TicketTy__6CD68451D9D2DD83");

            entity.Property(e => e.TicketTypeId).HasColumnName("TicketTypeID");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Voyage>(entity =>
        {
            entity.HasKey(e => e.VoyageId).HasName("PK__Voyages__577D73A37CF871A5");

            entity.HasIndex(e => new { e.RouteId, e.BoatId, e.DepartureTime, e.ArrivalTime }, "UQ_Voyage").IsUnique();

            entity.Property(e => e.VoyageId).HasColumnName("VoyageID");
            entity.Property(e => e.BoatId).HasColumnName("BoatID");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");

            entity.HasOne(d => d.Boat).WithMany(p => p.Voyages)
                .HasForeignKey(d => d.BoatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voyages_Boat");

            entity.HasOne(d => d.Route).WithMany(p => p.Voyages)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Voyages_Route");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
