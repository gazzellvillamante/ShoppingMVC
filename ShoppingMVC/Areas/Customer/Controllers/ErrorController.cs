using Microsoft.AspNetCore.Mvc;

namespace ShoppingMVC.Areas.Customer.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error404()
        {
            return View("NotFound");
        }

        public IActionResult Error500()
        {
            return View("Error");
        }
    }
}
