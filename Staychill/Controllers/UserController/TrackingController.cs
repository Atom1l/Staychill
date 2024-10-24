using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;
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
        public IActionResult TrackingResult(string shipmentCode)
        {
            // Find the tracking entry by shipment code
            var trackingEntry = _db.TrackingDB
                                    .Include(t => t.RetainCarts) // Include RetainCarts
                                    .ThenInclude(rc => rc.RetainCartItems) // Include RetainCartItems
                                    .FirstOrDefault(t => t.ShipmentCode == shipmentCode);

            if (trackingEntry == null)
            {
                return NotFound(); // Handle case where tracking entry is not found
            }

            return View(trackingEntry); // Pass the tracking entry to the view
        }

        public IActionResult TrackingIndex()
        {
            return View();
        }

        // ========== Merge CartItems to TrackingDB ========== //

        [HttpPost]
        public IActionResult CreateShipment(List<int> cartIds)
        {
            if (cartIds == null || !cartIds.Any())
            {
                return BadRequest("No cart items selected.");
            }

            // Log selected cart IDs
            foreach (var id in cartIds)
            {
                Console.WriteLine("Selected cartId: " + id);
            }

            // Generate shipment code
            string shipmentCode = Tracking.GenerateShipmentCode();

            // Create a new RetainCarts instance
            var retainCart = new RetainCarts
            {
                RetainCartItems = new List<RetainCartItem>()
            };

            // Process each selected cart item
            foreach (var cartId in cartIds)
            {
                var cartItem = _db.CartDB.Include(c => c.CartItems)
                                          .ThenInclude(ci => ci.Product)
                                          .FirstOrDefault(c => c.CartId == cartId);

                if (cartItem == null)
                {
                    continue;
                }

                // Add each cart item to RetainCarts
                foreach (var item in cartItem.CartItems)
                {
                    var retainCartItem = new RetainCartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        RetainCartId = retainCart.ReCartId // Ensure the foreign key is set correctly
                    };

                    retainCart.RetainCartItems.Add(retainCartItem);
                }

                // Remove the cart item from CartDB
                _db.CartDB.Remove(cartItem);
            }

            // Save RetainCart
            _db.RetaincartsDB.Add(retainCart);
            _db.SaveChanges();

            // Create tracking entry
            var tracking = new Tracking
            {
                ShipmentCode = shipmentCode,
                Status = "Pending",
                RetainCarts = new List<RetainCarts> { retainCart }
            };

            // Save tracking entry
            _db.TrackingDB.Add(tracking);
            _db.SaveChanges();

            // Redirect to the tracking result with the shipment code
            return RedirectToAction("TrackingResult", new { shipmentCode });
        }








    }
}
