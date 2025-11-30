using Microsoft.AspNetCore.Mvc;
using E_Commerce.Services.Model_View;
using E_Commerce.Data.Context;
using E_Commerce.Data.DataOrEntity;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_MVC.PL.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly E_CommerceDbContext _context;
        private const string CartSessionKey = "CartItems";

        public CheckoutController(E_CommerceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Check if user is logged in
            var isLoggedIn = HttpContext.Session.GetString("IsLoggedIn") == "true";
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Login");
            }

            // Get cart items
            var cartItems = GetCartItems();
            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Calculate totals
            var subtotal = cartItems.Sum(item => item.Price * item.Quantity);
            var tax = subtotal * 0.10m; // 10% tax
            var total = subtotal + tax;

            // Get user's saved shipping info
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.users.FirstOrDefault(u => u.Email == userEmail);

            var checkoutVM = new CheckoutVM
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                Tax = tax,
                Total = total
            };

            // Pre-fill with saved shipping info if available
            if (user != null && !string.IsNullOrEmpty(user.ShippingFirstName))
            {
                checkoutVM.FirstName = user.ShippingFirstName;
                checkoutVM.LastName = user.ShippingLastName;
                checkoutVM.StreetAddress = user.ShippingStreetAddress;
                checkoutVM.City = user.ShippingCity;
                checkoutVM.Phone = user.ShippingPhone;
            }

            return View(checkoutVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutVM model)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate cart items for the view
                var cartItems = GetCartItems();
                var subtotal = cartItems.Sum(item => item.Price * item.Quantity);
                var tax = subtotal * 0.10m;
                var total = subtotal + tax;

                model.CartItems = cartItems;
                model.Subtotal = subtotal;
                model.Tax = tax;
                model.Total = total;

                return View(model);
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Save shipping info if requested
            if (model.SaveInfo)
            {
                user.ShippingFirstName = model.FirstName;
                user.ShippingLastName = model.LastName;
                user.ShippingStreetAddress = model.StreetAddress;
                user.ShippingCity = model.City;
                user.ShippingPhone = model.Phone;
                await _context.SaveChangesAsync();
            }

            // Calculate totals from cart
            var orderCartItems = GetCartItems();
            var orderSubtotal = orderCartItems.Sum(item => item.Price * item.Quantity);
            var orderTax = orderSubtotal * 0.10m;
            var orderTotal = orderSubtotal + orderTax;

            // Create order
            var order = new Order
            {
                UserId = user.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                StreetAddress = model.StreetAddress,
                City = model.City,
                Phone = model.Phone,
                PaymentMethod = model.PaymentMethod,
                TotalAmount = orderTotal,
                Status = "Pending",
                CreationDate = DateTime.Now
            };

            _context.orders.Add(order);
            await _context.SaveChangesAsync();

            // Create order items
            foreach (var item in orderCartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.Id,
                    ProductName = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                    CreationDate = DateTime.Now
                };
                _context.orderItems.Add(orderItem);

                // Decrease stock
                var product = _context.products.FirstOrDefault(p => p.Id == item.Id);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    if (product.StockQuantity < 0) product.StockQuantity = 0;
                }
            }

            await _context.SaveChangesAsync();

            // Clear cart
            HttpContext.Session.Remove(CartSessionKey);

            TempData["OrderSuccess"] = $"Order #{order.Id} placed successfully!";
            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private List<CartItem> GetCartItems()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
        }
    }
}