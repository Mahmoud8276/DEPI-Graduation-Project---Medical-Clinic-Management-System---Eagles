using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using E_Commerce.Data.Context;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace E_Commerce_MVC.PL.Controllers
{
    public class LoginController : Controller
    {
        private readonly E_CommerceDbContext _context;
        public LoginController(E_CommerceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"];
            return View();
        }

        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }
            string passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            var user = _context.users.FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
            if (user != null)
            {
                HttpContext.Session.SetString("IsLoggedIn", "true");
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserEmail", user.Email);
                return RedirectToAction("Index", "Home");
            }
            // Admin fallback (optional)
            if (email == "admin@admin.com" && password == "AdminPassword123")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}