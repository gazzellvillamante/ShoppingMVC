using Microsoft.AspNetCore.Mvc;

namespace ShoppingMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/Error")]
    public class ErrorController : Controller
    {
        [Route("Error{code}")]
        public IActionResult ErrorHandler(int code)
        {
            if (code == 404)
                return View("NotFound");
            if (code == 500)
                return View("Error");

            // Optionally handle other codes
            return View("GenericError");
        }

        [Route("/Error")]
        public IActionResult Error()
        {
            return View("Error");
        }

        [Route("Customer/Error")]
        public IActionResult Crash()
        {
            throw new Exception("This is a test crash to trigger 500 error.");
        }
    }
}
