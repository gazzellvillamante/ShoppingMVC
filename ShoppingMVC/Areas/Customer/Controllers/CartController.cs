﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingMVC.DataAccess.Repository;
using ShoppingMVC.DataAccess.Repository.IRepository;
using ShoppingMVC.Models;
using ShoppingMVC.Extensions;
using ShoppingMVC.Models.ViewModels;

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
            return View(); // Checkout
        }

        [Route("Cart/OrderConfirmation")]
        public async Task<IActionResult> OrderConfirmation()
        {
            // Retrieve the shipping details from the session
            var shippingDetails = HttpContext.Session.GetObjectFromJson<ShippingDetails>("ShippingDetails");

            // Retrieve the user's cart (which now includes CartDetails)
            var cart = await _cartRepo.GetUserCart();

            if (shippingDetails != null && cart != null && cart.CartDetails != null && cart.CartDetails.Any())
            {
                // Create the view model
                var confirmationViewModel = new OrderConfirmationVM
                {
                    ShippingDetails = shippingDetails,
                    CartDetails = cart.CartDetails.ToList(),
                    OrderTotal = cart.CartDetails.Sum(item => item.Product.ListPrice * item.Quantity)
                };

                return View(confirmationViewModel);
            }
            else
            {
                return RedirectToAction("GetUserCart");
            }
        }

        [HttpPost]
        [Route("Cart/Checkout")]
        public IActionResult Checkout(ShippingDetails shippingDetails)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetObjectAsJson("ShippingDetails", shippingDetails);
                return RedirectToAction("OrderConfirmation");
            }

            
            return View(shippingDetails);
        }
    }
}
