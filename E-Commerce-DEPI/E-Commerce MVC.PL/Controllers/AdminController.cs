using Microsoft.AspNetCore.Mvc;
using E_Commerce.Data.Context;
using E_Commerce.Data.DataOrEntity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_MVC.PL.Controllers
{
    public class AdminController : Controller
    {
        private readonly E_CommerceDbContext _context;

        public AdminController(E_CommerceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Check if user is admin
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            if (!isAdmin)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public IActionResult Orders()
        {
            // Check if user is admin
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            if (!isAdmin)
                return RedirectToAction("Index", "Login");

            var orders = _context.orders
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .OrderByDescending(o => o.CreationDate)
                .ToList();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptOrder([FromBody] AcceptDeclineOrderRequest request)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            if (!isAdmin)
                return Json(new { success = false, message = "Unauthorized" });

            var order = await _context.orders.FindAsync(request.orderId);
            if (order == null)
                return Json(new { success = false, message = "Order not found" });

            order.Status = "Accepted";
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order accepted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> DeclineOrder([FromBody] AcceptDeclineOrderRequest request)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";
            if (!isAdmin)
                return Json(new { success = false, message = "Unauthorized" });

            var order = await _context.orders.FindAsync(request.orderId);
            if (order == null)
                return Json(new { success = false, message = "Order not found" });

            order.Status = "Declined";
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order declined successfully" });
        }

        public class AcceptDeclineOrderRequest
        {
            public int orderId { get; set; }
        }
    }
}