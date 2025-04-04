using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.DataAccess.Repository;
using ShoppingMVC.DataAccess.Repository.IRepository;

namespace ShoppingMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        [Route("Cart/AddItem")]
        public async Task<IActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(productId, qty);  // Ensure repository handles updating the quantity.
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }


        // RemoveItem Action
        [Route("Cart/RemoveItem")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cartRepo.RemoveItem(productId);  // Ensure repository handles quantity decrement.
            return RedirectToAction("GetUserCart");
        }


        [Route("Cart/GetUserCart")]
        public async Task<IActionResult> GetUserCart()
        {            
            var cart = await _cartRepo.GetUserCart();
           
            return View(cart);
        }

        [Route("Cart/GetTotalItemInCart")]
        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        [Route("Cart/Checkout")]
        public IActionResult Checkout()
        {
            TempData["success"] = "Category created successfully";
            return View("Index");
        }

        /**

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartRepo.DoCheckout(model);
            if (!isCheckedOut)
                return RedirectToAction(nameof(OrderFailure));
            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
       **/
    }
}
