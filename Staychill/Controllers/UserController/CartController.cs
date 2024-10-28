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

        // ====================================================================================================================== //

        // ========== APPLYDISCOUNT ========== //
        [HttpPost]
        public IActionResult ApplyDiscount(CartViewModel model) // Click the apply discount to preceed this action //
        {
            {

                var discount = _db.DiscountDB.FirstOrDefault(d => d.DiscountCode == model.DiscountCode); // Retrieve the discountCode data //

                var cartitems = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product)
                                          .Include(c => c.CartItems).ThenInclude(c => c.Product.Images).ToList(); // Retrieve the CartDB data //

                model.TotalAmount = cartitems.SelectMany(c => c.CartItems).Sum(item => item.TotalPrice); // Calculate the total price of the product(s) //

                if (discount != null) // If Discount code is exist //
                {
                    model.DiscountAmount = discount.DiscountAmount; // CartViewModel.DiscountAmount = discount(DiscountDB).DiscountAmount to receive the amount of discount //
                    model.TotalDiscountAmount = model.CalculatedDiscountAmount; // Method to calculate the discount amount with total price but not the final result yet //
                    model.DiscountedTotal = model.TotalAmount - model.CalculatedDiscountAmount; // Final Total Price //
                }
                else // If Discount code is not exist //
                {
                    model.DiscountedTotal = model.TotalAmount; // No discount applied //
                }

                model.CartItemDetails = cartitems;


                return View("CartIndex", model); // Return to CartIndex with new CartViewModel that got update Discount data //
            }

        }
        // ========== APPLYDISCOUNT ========== //

        // ====================================================================================================================== //

        // ========== QUANTITIES BUTTON ========== //
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, string action) // To make user can edit quantities of the product at CartIndex //
        {
            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Product).FirstOrDefault(); // Retrieve the Cartitem data //
            var item = cart.CartItems.FirstOrDefault(c => c.ProductId == productId); // Find if the Id parameter that we send through is matching with CartItem Id //
            if (item != null) // If item not null //
            {
                if (action == "increase" && item.Quantity <= item.Product.Instock) // button with action "increase" and the amount of quantity still not more that inStock //
                {
                    item.Quantity ++;
                }
                else if (action == "decrease" && item.Quantity > 1) // button with action "decrease" and the amount of quantity still not less than 1 //
                {
                    item.Quantity --;
                }
                _db.SaveChanges();
            }

            return RedirectToAction("CartIndex"); // Return to CartIndex | No need to add beside parameter because this is directly edit through database not viewModel //
        }
        // ========== QUANTITIES BUTTON ========== //

        // ====================================================================================================================== //

        // ========== REMOVEFROMCART ========== //
        [HttpPost]
        public IActionResult CartRemove(int RemovecartId, int[] RemoveitemId)
        {
            // Find the cart using the CartId and check if Id is matching //
            var cart = _db.CartDB
                .Include(c => c.CartItems)
                .ThenInclude(c => c.Product)
                .FirstOrDefault(p => p.CartId == RemovecartId);

            if (cart != null)
            {
                // Remove each CartItem that matches the provided CartItemIds
                foreach (var itemId in RemoveitemId)
                {
                    var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == itemId); // Check the ItemId to see the Item detail | ItemId is not a CartId //
                    if (itemToRemove != null)
                    {
                        itemToRemove.Product.Instock += itemToRemove.Quantity; // Add the amount of quantity back to stock //
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
