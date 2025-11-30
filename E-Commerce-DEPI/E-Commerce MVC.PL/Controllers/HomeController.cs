using E_Commerce_MVC.PL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using E_Commerce.Services.Interfaces;
using E_Commerce.Services.Model_View;
using System.Linq;

namespace E_Commerce_MVC.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var allDiscounted = (await _productService.GetBestSalesProductsAsync(20)).ToList();
            var sliderProducts = allDiscounted.Take(8).ToList();
            var bestDiscountProducts = allDiscounted.Skip(8).Take(4).ToList();
            var newestProducts = (await _productService.GetNewestProductsAsync(8)).ToList();
            var types = await _productService.GetAllTypesAsync();

            ViewBag.SliderProducts = sliderProducts;
            ViewBag.BestDiscountProducts = bestDiscountProducts;
            ViewBag.NewestProducts = newestProducts;
            ViewBag.Types = types;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}