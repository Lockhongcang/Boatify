using Boatify.Models;
using Microsoft.EntityFrameworkCore;

namespace Boatify.Services
{
    public class DatabaseInitializer
    {
        private readonly BoatifyContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(BoatifyContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // Ensure database is created
                await _context.Database.EnsureCreatedAsync();
                _logger.LogInformation("Database created successfully");

                // Check if we need to seed data
                if (!await _context.Users.AnyAsync())
                {
                    await SeedDataAsync();
                    _logger.LogInformation("Database seeded successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing database");
                throw;
            }
        }

        private async Task SeedDataAsync()
        {
            // Create test users
            var testUsers = new List<User>
            {
                new User
                {
                    Email = "test@example.com",
                    Password = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "secret"
                    FullName = "Test User",
                    Phone = "0123456789",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Email = "admin@boatify.com",
                    Password = "jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=", // "secret"
                    FullName = "Admin User",
                    Phone = "0987654321",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            };

            _context.Users.AddRange(testUsers);
            await _context.SaveChangesAsync();

            // Create sample orders for testing
            var testUser = testUsers.First();
            var sampleOrders = new List<Order>
            {
                new Order
                {
                    UserId = testUser.UserId,
                    OrderCode = "BT20241201001",
                    Departure = "Hà Nội",
                    Destination = "Hải Phòng",
                    DepartureDate = DateTime.Now.AddDays(7),
                    DepartureTime = "08:00",
                    TotalAmount = 500000,
                    Status = "Paid",
                    CreatedAt = DateTime.UtcNow,
                    ContactName = testUser.FullName,
                    ContactEmail = testUser.Email,
                    ContactPhone = testUser.Phone,
                    ExternalVoyageId = 1,
                    ExternalScheduleId = 1,
                    ExternalRouteId = 1
                },
                new Order
                {
                    UserId = testUser.UserId,
                    OrderCode = "BT20241201002",
                    Departure = "TP.HCM",
                    Destination = "Vũng Tàu",
                    DepartureDate = DateTime.Now.AddDays(14),
                    DepartureTime = "09:30",
                    TotalAmount = 750000,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                    ContactName = testUser.FullName,
                    ContactEmail = testUser.Email,
                    ContactPhone = testUser.Phone,
                    ExternalVoyageId = 2,
                    ExternalScheduleId = 2,
                    ExternalRouteId = 2
                }
            };

            _context.Orders.AddRange(sampleOrders);
            await _context.SaveChangesAsync();

            // Create sample tickets
            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    OrderId = sampleOrders[0].OrderId,
                    SeatCode = "A01",
                    PassengerName = "Nguyễn Văn A",
                    PassengerPhone = "0123456789",
                    PassengerIdCard = "123456789",
                    Price = 250000,
                    TicketType = "Adult",
                    TicketClass = "Economy",
                    CreatedAt = DateTime.UtcNow,
                    ExternalSeatId = 1
                },
                new Ticket
                {
                    OrderId = sampleOrders[0].OrderId,
                    SeatCode = "A02",
                    PassengerName = "Trần Thị B",
                    PassengerPhone = "0987654321",
                    PassengerIdCard = "987654321",
                    Price = 250000,
                    TicketType = "Adult",
                    TicketClass = "Economy",
                    CreatedAt = DateTime.UtcNow,
                    ExternalSeatId = 2
                }
            };

            _context.Tickets.AddRange(tickets);
            await _context.SaveChangesAsync();

            // Create sample payments
            var payments = new List<Payment>
            {
                new Payment
                {
                    OrderId = sampleOrders[0].OrderId,
                    PaymentMethod = "VNPay",
                    Amount = 500000,
                    Status = "Success",
                    TransactionId = "VNP_" + DateTime.Now.Ticks,
                    CreatedAt = DateTime.UtcNow,
                    PaidAt = DateTime.UtcNow
                },
                new Payment
                {
                    OrderId = sampleOrders[1].OrderId,
                    PaymentMethod = "MoMo",
                    Amount = 750000,
                    Status = "Pending",
                    TransactionId = "MOMO_" + DateTime.Now.Ticks,
                    CreatedAt = DateTime.UtcNow
                }
            };

            _context.Payments.AddRange(payments);
            await _context.SaveChangesAsync();
        }
    }
}
