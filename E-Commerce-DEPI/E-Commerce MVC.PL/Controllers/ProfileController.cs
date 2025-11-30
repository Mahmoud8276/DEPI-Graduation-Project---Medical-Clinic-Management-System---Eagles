using Microsoft.AspNetCore.Mvc;
using E_Commerce.Data.Context;
using E_Commerce.Data.DataOrEntity;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_MVC.PL.Controllers
{
    public class ProfileController : Controller
    {
        private readonly E_CommerceDbContext _context;
        public ProfileController(E_CommerceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Index", "Login");
            var user = _context.users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Index", "Login");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int id, string firstName, string lastName, string name, string email, string address, string currentPassword, string newPassword, string confirmNewPassword)
        {
            var user = _context.users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                ViewBag.Error = "User not found.";
                return View(user);
            }
            // Update name fields
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Name = firstName + " " + lastName;
            }
            else if (!string.IsNullOrWhiteSpace(name))
            {
                user.Name = name;
                user.FirstName = null;
                user.LastName = null;
            }
            // Update email
            if (!string.IsNullOrWhiteSpace(email))
                user.Email = email;
            // Update address
            user.Address = address;
            // Password change
            if (!string.IsNullOrWhiteSpace(currentPassword) && !string.IsNullOrWhiteSpace(newPassword))
            {
                string currentHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(currentPassword)));
                if (user.PasswordHash != currentHash)
                {
                    ViewBag.Error = "Current password is incorrect.";
                    return View(user);
                }
                if (newPassword != confirmNewPassword)
                {
                    ViewBag.Error = "New passwords do not match.";
                    return View(user);
                }
                user.PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(newPassword)));
            }
            await _context.SaveChangesAsync();
            ViewBag.Success = "Profile updated successfully.";
            return View(user);
        }

        public IActionResult MyOrders()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Index", "Login");

            var user = _context.users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Index", "Login");

            var orders = _context.orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.CreationDate)
                .ToList();

            return View(orders);
        }
    }
}