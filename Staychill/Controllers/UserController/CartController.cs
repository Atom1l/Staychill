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

        // ========== DISPLAY ========== //
        public IActionResult CartIndex() // Display the cart view for user //
        {
            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product.Images).ToList(); // Include all Model that link with CartDB into list //

            var viewModel = new CartViewModel
            {
                CartItemDetails = cart, // CartItemDetails as a list<> will contain "cart" data that include all data from CartDb //
                TotalAmount = cart.SelectMany(c => c.CartItems).Select(item => item.TotalPrice - (item.Discount?.DiscountAmount ?? 0)).Sum() // Calculate the TotalAmount //
            };
            return View(viewModel); // return with "viewModel" value // 
        }

        [HttpPost]
        public IActionResult CartIndex(int productId, int quantity) // Add Product to the CartIndex(GET) //
        {
            var product = _db.ProductDB.FirstOrDefault(p => p.Id == productId); // Check if productId matches with ProductDB.Id //

            // If null return to Product Page //
            if (product == null)
            {
                return RedirectToAction("ProductIndex", "TestUserCreatingAccount");
            }

            // Check if the quantity is more than Instock of ProductDB or not //
            if (product.Instock < quantity)
            {
                return RedirectToAction("ProductIndex","TestUserCreatingAccount"); // Make user go back to Product Page and type again till quantity is <= Instock //
            }

            // If the workflow is fine then proceed these action //
            // Find the existing Cart
            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product).FirstOrDefault(); // Get the first Cart (since there is only one cart) //

            // If the cart does not exist Create a new one instead //
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
      
            product.Instock -= quantity; // Deduct the stock quantities and save the cart //
            _db.SaveChanges(); // Save changes //

            return RedirectToAction("CartIndex");
        }
        // ========== DISPLAY ========== //


        // ========== APPLYDISCOUNT ========== //
        [HttpPost]
        public IActionResult ApplyDiscount(CartViewModel model)
        {
            {

                var discount = _db.DiscountDB.FirstOrDefault(d => d.DiscountCode == model.DiscountCode);

                var cartitems = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product)
                                          .Include(c => c.CartItems).ThenInclude(c => c.Product.Images).ToList();

                model.TotalAmount = cartitems.SelectMany(c => c.CartItems).Sum(item => item.TotalPrice);

                if (discount != null)
                {
                    model.DiscountAmount = discount.DiscountAmount;
                    model.TotalDiscountAmount = model.CalculatedDiscountAmount;
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
        // ========== APPLYDISCOUNT ========== //



        // ========== QUANTITIES BUTTON ========== //
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string action)
        {
            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product).FirstOrDefault();
            var item = cart.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (action == "increase" && item.Quantity <= item.Product.Instock)
                {
                    item.Quantity ++;
                }
                else if (action == "decrease" && item.Quantity > 1)
                {
                    item.Quantity --;
                }
                _db.SaveChanges();
            }

            return RedirectToAction("CartIndex");
        }

        // ========== QUANTITIES BUTTON ========== //



        // ========== REMOVEFROMCART ========== //
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
                        itemToRemove.Product.Instock += itemToRemove.Quantity; // Add back the stock by a quantity value //
                        cart.CartItems.Remove(itemToRemove); // Remove the item from the cart //
                    }
                }
                _db.SaveChanges(); // Save changes to the database
            }

            return RedirectToAction("CartIndex"); // Redirect to the appropriate action
        }
        // ========== REMOVEFROMCART ========== //
    }
}
