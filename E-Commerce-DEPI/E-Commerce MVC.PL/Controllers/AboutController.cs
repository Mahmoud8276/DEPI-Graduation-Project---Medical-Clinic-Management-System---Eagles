using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.PL.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
} 