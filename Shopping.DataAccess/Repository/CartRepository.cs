using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingMVC.DataAccess.Data;
using ShoppingMVC.Models;
using ShoppingMVC.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingMVC.DataAccess.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) 
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User is not logged in");
                }

                // Get the user's shopping cart (or create a new one if none exists)
                var cart = await GetCart(userId);
                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                    await _db.SaveChangesAsync(); // Commit the cart creation first
                }

                // Check if the product already exists in the cart
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);

                if (cartItem != null)
                {
                    // Product already exists in the cart, increase the quantity
                    cartItem.Quantity += qty;
                }
                else
                {
                    // Product does not exist in the cart, add it as a new item
                    var product = await _db.Products.FindAsync(productId);
                    if (product == null)
                    {
                        throw new InvalidOperationException("Product not found");
                    }

                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        ListPrice = product.ListPrice // Make sure to store the product's price
                    };
                    _db.CartDetails.Add(cartItem);
                }

                // Save changes to the cart details
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the updated item count in the cart
                var cartItemCount = await GetCartItemCount(userId);
                return cartItemCount;
            }
            catch (Exception ex)
            {
                // Handle the exception (you may log it here if necessary)
                await transaction.RollbackAsync();
                throw new InvalidOperationException("Error occurred while adding item to the cart", ex);
            }
        }



        public async Task<int> RemoveItem(int productId)
        {
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User is not logged in");
                }

                var cart = await GetCart(userId);
                if (cart == null)
                {
                    throw new InvalidOperationException("Invalid cart");
                }

                // Find the cart item to remove or reduce quantity
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem == null)
                {
                    throw new InvalidOperationException("Product not found in cart");
                }

                // Decrease the quantity or remove the product completely
                if (cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= 1; // Decrease the quantity by 1
                }

                // Save changes to the cart
                await _db.SaveChangesAsync();

                // Return the updated item count in the cart
                var cartItemCount = await GetCartItemCount(userId);
                return cartItemCount;
            }
            catch (Exception ex)
            {
                // Handle the exception (you may log it here if necessary)
                throw new InvalidOperationException("Error occurred while removing item from the cart", ex);
            }
        }


        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new InvalidOperationException("Invalid userid");
            }                
            var shoppingCart = await _db.ShoppingCarts
                                  .Include(a => a.CartDetails)
                                  .ThenInclude(a => a.Product)
                                  .ThenInclude(a => a.Category)
                                  .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;

        }
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) 
            {
                userId = GetUserId();
            }

            var data = await (from cart in _db.ShoppingCarts
                              join cartDetail in _db.CartDetails
                              on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId 
                              select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }

        

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
