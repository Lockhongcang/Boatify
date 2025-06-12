using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Boatify.Models;
using Boatify.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Boatify.Controllers
{
    public class AccountController : BaseController
    {
        private readonly BoatifyContext _context;

        public AccountController(BoatifyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            TempData["FormState"] = "login";
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string passwordHash = ComputeSha256Hash(model.Password);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == passwordHash && u.IsActive);

            if (user == null)
            {
                SetError("Sai email hoặc mật khẩu.");
                return View(model);
            }

            // Use session instead of claims for simplicity
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            SetSuccess($"Xin chào, {user.FullName}");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            SetSuccess("Đăng xuất thành công!");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            TempData["FormState"] = "register";
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                SetError("Email đã tồn tại.");
                return View(model);
            }

            var user = new User
            {
                Email = model.Email,
                Password = ComputeSha256Hash(model.Password),
                FullName = model.FullName,
                Phone = model.Phone,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            SetSuccess("Tạo tài khoản thành công. Vui lòng đăng nhập.");
            return RedirectToAction("Login");
        }

        private string ComputeSha256Hash(string raw)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
