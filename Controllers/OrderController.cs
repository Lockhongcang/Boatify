using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Boatify.Models;
using Boatify.Models.ViewModels;
using Boatify.Models.Dto;

namespace Boatify.Controllers
{
    public class OrderController : BaseController
    {
        private readonly BoatifyContext _context;
        private readonly ILogger<OrderController> _logger;

        public OrderController(BoatifyContext context, ILogger<OrderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem lịch sử đặt vé.";
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index", "Home");
            }

            var orders = await _context.Orders
                .Include(o => o.Tickets)
                .Include(o => o.Payments)
                .Where(o => o.UserId == user.UserId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            var orderSummaries = orders.Select(o => new OrderSummary
            {
                OrderId = o.OrderId,
                OrderCode = o.OrderCode,
                OrderDate = o.CreatedAt,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Departure = o.Departure,
                Destination = o.Destination,
                DepartureTime = o.DepartureDate,
                Tickets = o.Tickets.Select(t => new TicketInfo
                {
                    SeatCode = t.SeatCode,
                    PassengerName = t.PassengerName,
                    PassengerPhone = t.PassengerPhone,
                    Price = t.Price
                }).ToList(),
                Payment = new PaymentInfo
                {
                    Method = o.Payments.FirstOrDefault()?.PaymentMethod ?? "N/A",
                    Status = o.Payments.FirstOrDefault()?.Status ?? "N/A",
                    PaidDate = o.Payments.FirstOrDefault()?.PaidAt,
                    Amount = o.Payments.FirstOrDefault()?.Amount ?? 0
                }
            }).ToList();

            return View(orderSummaries);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem chi tiết đơn hàng.";
                return RedirectToAction("Login", "Account");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                return RedirectToAction("Index", "Home");
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Tickets)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == user.UserId);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index");
            }

            var orderSummary = new OrderSummary
            {
                OrderId = order.OrderId,
                OrderCode = order.OrderCode,
                OrderDate = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Departure = order.Departure,
                Destination = order.Destination,
                DepartureTime = order.DepartureDate,
                Tickets = order.Tickets.Select(t => new TicketInfo
                {
                    SeatCode = t.SeatCode,
                    PassengerName = t.PassengerName,
                    PassengerPhone = t.PassengerPhone,
                    Price = t.Price
                }).ToList(),
                Payment = new PaymentInfo
                {
                    Method = order.Payments.FirstOrDefault()?.PaymentMethod ?? "N/A",
                    Status = order.Payments.FirstOrDefault()?.Status ?? "N/A",
                    PaidDate = order.Payments.FirstOrDefault()?.PaidAt,
                    Amount = order.Payments.FirstOrDefault()?.Amount ?? 0
                }
            };

            return View(orderSummary);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, message = "Vui lòng đăng nhập." });
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy thông tin người dùng." });
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id && o.UserId == user.UserId);

            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng." });
            }

            // Check if order can be cancelled (e.g., not within 24 hours of departure)
            var hoursUntilDeparture = (order.DepartureDate - DateTime.Now).TotalHours;
            if (hoursUntilDeparture < 24)
            {
                return Json(new { success = false, message = "Không thể hủy vé trong vòng 24 giờ trước giờ khởi hành." });
            }

            if (order.Status == "Paid")
            {
                order.Status = "Cancelled";
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
            }
            else
            {
                return Json(new { success = false, message = "Chỉ có thể hủy đơn hàng đã thanh toán." });
            }
        }


    }
}
