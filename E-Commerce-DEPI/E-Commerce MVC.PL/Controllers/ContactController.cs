using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace E_Commerce_MVC.PL.Controllers
{
    public class ContactController : Controller
    {
        // In-memory storage for demo (replace with DB in production)
        private static List<ContactMessage> Messages = new List<ContactMessage>();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                ViewBag.Messages = Messages;
            }
            return View(); // This will look for Views/Contact/Contact.cshtml
        }

        [HttpPost]
        public IActionResult Index(string Name, string Email, string Phone, string Message)
        {
            Messages.Add(new ContactMessage { Name = Name, Email = Email, Phone = Phone, Message = Message });
            if (HttpContext.Session.GetString("IsAdmin") == "true")
            {
                ViewBag.Messages = Messages;
            }
            return View();
        }
    }

    public class ContactMessage
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}