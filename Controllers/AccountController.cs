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

            var user = _context.Accounts.FirstOrDefault(a => a.Email == model.Email && a.PasswordHash == passwordHash);

            if (user == null)
            {
                SetError("Sai email hoặc mật khẩu.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("UserId", user.AccountId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal, new AuthenticationProperties
            {
                IsPersistent = model.RememberMe // tự động nhớ
            });

            SetSuccess($"Xin chào, {user.Email}");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Login");
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

            if (_context.Accounts.Any(a => a.Email == model.Email))
            {
                SetError("Email đã tồn tại.");
                return View(model);
            }

            var account = new Account
            {
                Email = model.Email,
                PasswordHash = ComputeSha256Hash(model.Password),
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();

            string[] names = model.FullName.Trim().Split(' ', 2);
            var customer = new Customer
            {
                AccountId = account.AccountId,
                FirstName = names.Length > 1 ? names[0] : "",
                LastName = names.Length > 1 ? names[1] : names[0],
                PhoneNumber = model.Phone
            };
            _context.Customers.Add(customer);
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
