using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using Staychill.Data;
using Staychill.Models;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.DiscountModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.Models.UserModel;
using Staychill.ViewModel;

namespace Staychill.Controllers.UserController
{
    public class TrackingController : Controller
    {
        private readonly StaychillDbContext _db;
        public TrackingController(StaychillDbContext db)
        {
            _db = db;
        }
        // ========== DISPLAY ========== //
        public IActionResult TrackingResult(string shipmentCode, string status, int cartId)
        {
            // Retrieve the tracking information based on the shipment code or cart ID
            var tracking = _db.TrackingDB.Include(t => t.RetainCart)
                                          .ThenInclude(c => c.Product)
                                          .FirstOrDefault(t => t.ShipmentCode == shipmentCode);

            if (tracking == null)
            {
                return NotFound(); // Return a 404 if the tracking information is not found
            }

            return View(tracking); // Pass the tracking information to the view
        }

        public IActionResult TrackingIndex()
        {
            return View();
        }

        // ========== DISPLAY ========== //

        // ========== Merge CartItems to TrackingDB ========== //
        [HttpPost]
        public IActionResult CreateShipment(int cartId)
        {
            // Find the cart item based on the cartId
            var cartItem = _db.CartDB.Include(c => c.Product).FirstOrDefault(c => c.CartId == cartId);

            if (cartItem == null)
            {
                return NotFound(); // Return a 404 if the cart item is not found
            }

            // Create a new RetainCart entry using the cartItem data
            var retainCart = new RetainCart
            {
                ProductId = cartItem.ProductId, // Link to the product
                Quantity = cartItem.Quantity, // Keep the quantity from the cart
                UnitPrice = cartItem.UnitPrice, // Keep the unit price from the cart
            };

            // Save the new retain cart entry to the database
            _db.RetainCartDB.Add(retainCart);
            _db.SaveChanges(); // This should automatically generate CartId

            // Create a new Tracking entry
            var tracking = new Tracking
            {
                CartId = retainCart.CartId, // Use the newly created RetainCart's ID
                Status = "Pending",
            };

            // Save the new tracking entry to the database
            _db.TrackingDB.Add(tracking);
            _db.SaveChanges(); // Save changes

            // Remove the cart item from CartDB
            _db.CartDB.Remove(cartItem);
            _db.SaveChanges(); // Save changes again to remove cart item

            // Redirect or return a view
            return RedirectToAction("TrackingResult", new { shipmentCode = tracking.ShipmentCode, status = tracking.Status, cartId = retainCart.CartId });
        }



    }
}
