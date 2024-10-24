using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.DiscountModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.ViewModel;

namespace Staychill.Controllers.UserController
{
    public class CartController : Controller
    {
        private readonly StaychillDbContext _db;
        public CartController(StaychillDbContext db)
        {
            _db = db;
        }

        public IActionResult CartIndex()
        {
            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product).ToList();

            var viewModel = new CartViewModel
            {
                CartItemDetails = cart,
                TotalAmount = cart.SelectMany(c => c.CartItems).Select(item => item.TotalPrice - (item.Discount?.DiscountAmount ?? 0)).Sum()
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult CartIndex(int productId, int quantity)
        {
            var product = _db.ProductDB.FirstOrDefault(p => p.Id == productId); // Check if productId matches with ProductDB.Id
            if (product == null)
            {
                return RedirectToAction("ProductIndex", "TestUserCreatingAccount");
            }

            // Find the existing Cart (assuming there's only one cart on the website)
            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product).FirstOrDefault(); // Get the first Cart (since there is only one cart)

            // If the cart does not exist, you might want to create one here or handle the scenario
            if (cart == null)
            {
                cart = new Cart(); // Create a new Cart if one doesn't exist
                _db.CartDB.Add(cart); // Add it to the database
                _db.SaveChanges(); // Save changes to generate CartId
            }

            // Check if the product is already in the cart
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity; // If matching, increase the quantity
            }
            else
            {
                // Create a new CartItem
                var newCartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price ?? 0, // Use null-coalescing to set to 0 if null
                    CartId = cart.CartId // Associate the CartItem with the existing Cart
                };
                cart.CartItems.Add(newCartItem); // Add new CartItem to the Cart
            }

            _db.SaveChanges(); // Save changes

            return RedirectToAction("CartIndex");
        }


        [HttpPost]
        public IActionResult ApplyDiscount(CartViewModel model)
        {
            {

                var discount = _db.DiscountDB.FirstOrDefault(d => d.DiscountCode == model.DiscountCode);

                var cartitems = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product).ToList();

                model.TotalAmount = cartitems.SelectMany(c => c.CartItems).Sum(item => item.TotalPrice);

                if (discount != null)
                {
                    model.DiscountAmount = discount.DiscountAmount;
                    model.DiscountedTotal = model.TotalAmount - model.CalculatedDiscountAmount;
                }
                else
                {
                    model.DiscountedTotal = model.TotalAmount; // No discount applied
                }

                model.CartItemDetails = cartitems;

                return View("CartIndex", model);
            }

        }

        [HttpPost]
        public IActionResult CartRemove(int RemovecartId, int[] RemoveitemId)
        {
            // Find the cart using the CartId
            var cart = _db.CartDB
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefault(p => p.CartId == RemovecartId);

            if (cart != null)
            {
                // Remove each CartItem that matches the provided CartItemIds
                foreach (var itemId in RemoveitemId)
                {
                    var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemId);
                    if (itemToRemove != null)
                    {
                        // Remove the item from the cart
                        cart.CartItems.Remove(itemToRemove);
                    }
                }
                _db.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("CartIndex"); // Redirect to the appropriate action
        }
    }
}
