using Microsoft.AspNetCore.Mvc;
using E_Commerce.Data.DataOrEntity;
using System.Text.Json;

namespace E_Commerce_MVC.PL.Controllers
{
    public class WishlistController : Controller
    {
        private const string WishlistSessionKey = "WishlistItems";

        public IActionResult Index()
        {
            var wishlistItems = GetWishlistItems();
            return View(wishlistItems);
        }

        [HttpPost]
        public IActionResult AddToWishlist([FromForm] CartItem item)
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true" || HttpContext.Session.GetString("IsAdmin") == "true")
            {
                return Json(new { success = false, message = "You must be logged in as a user to add to wishlist." });
            }
            var wishlistItems = GetWishlistItems();
            if (!wishlistItems.Any(i => i.Id == item.Id))
            {
                wishlistItems.Add(item);
                SaveWishlistItems(wishlistItems);
            }
            return Json(new { success = true, message = "Product added to wishlist!" });
        }

        [HttpPost]
        public IActionResult RemoveFromWishlist(int id)
        {
            var wishlistItems = GetWishlistItems();
            var itemToRemove = wishlistItems.FirstOrDefault(i => i.Id == id);
            if (itemToRemove != null)
            {
                wishlistItems.Remove(itemToRemove);
                SaveWishlistItems(wishlistItems);
                return Json(new { success = true, message = "Product removed from wishlist!" });
            }
            return Json(new { success = false, message = "Product not found in wishlist." });
        }

        [HttpPost]


        private List<CartItem> GetWishlistItems()
        {
            var wishlistJson = HttpContext.Session.GetString(WishlistSessionKey);
            return wishlistJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(wishlistJson);
        }

        private void SaveWishlistItems(List<CartItem> wishlistItems)
        {
            var wishlistJson = JsonSerializer.Serialize(wishlistItems);
            HttpContext.Session.SetString(WishlistSessionKey, wishlistJson);
        }
    }
}