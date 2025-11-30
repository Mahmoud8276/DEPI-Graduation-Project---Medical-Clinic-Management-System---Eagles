using Microsoft.AspNetCore.Mvc;
using E_Commerce.Data.Context;
using E_Commerce.Data.DataOrEntity;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace E_Commerce_MVC.PL.Controllers
{
    public class SignUpController : Controller
    {
        private readonly E_CommerceDbContext _context;
        public SignUpController(E_CommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string name, string email, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "All fields are required.";
                return View();
            }
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }
            if (_context.users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email already registered.";
                return View();
            }
            // Simple hash for demo
            string passwordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
            var user = new User { Name = name, Email = email, PasswordHash = passwordHash };
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Account created! Please log in.";
            return RedirectToAction("Index", "Login");
        }
    }
}