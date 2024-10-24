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
        public IActionResult TrackingResult(string shipmentCode) // Display an items and it status after User fill the input in TrackingIndex or Clicked Payment //
        {
            // Retrieve the tracking entry based on the shipment code to a variable track //
            var track = _db.TrackingDB.Include(t => t.RetainCarts).ThenInclude(rc => rc.RetainCartItems).FirstOrDefault(t => t.ShipmentCode == shipmentCode);

            // Check if track exists //
            if (track == null)
            {
                return NotFound($"Tracking information for shipment code {shipmentCode} not found."); // Show the Information that ShipmentCode is not found //
            }

            // Map the data to the ViewModel | Map in this condition is like replace a TrackingResultViewModel data with "track" data //
            var viewModel = new TrackingResultViewModel
            {
                ShipmentCode = track.ShipmentCode,
                Status = track.Status,
                // Merge the RetainCartItems into flattened (Make Complex data structure become single Level | Easy for reaching) //
                RetainCartItems = track.RetainCarts.SelectMany(rc => rc.RetainCartItems)
                    .Select(item => new RetainCartItemViewModel
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    }).ToList()
            };

            // Return the view with the "viewModel" //
            return View(viewModel);
        }

        public IActionResult TrackingIndex()
        {
            return View(); // Create a View to fill the shipmentCode in input to Redirect to "TrackingResult" //
        }
        // ========== DISPLAY ========== //

        // =========================================================================================================================================== //

        // ========== MERGE CART ITEMS TO TRACKINGDB ========== //
        [HttpPost]
        public IActionResult CreateShipment(int[] cartIds, int[] quantities, float[] unitPrices) // List bc 1 Cart can contain many products //
        {
            // Check if any items are selected //
            if (cartIds == null || cartIds.Length == 0)
            {
                return BadRequest("No items selected.");
            }

            // Create a new RetainCarts variable to store RetainCartItems
            var retainCart = new RetainCarts
            {
                RetainCartItems = new List<RetainCartItem>()
            };

            // Generate shipment code | 8 digit random between A-Z,0-9 //
            string shipmentCode = Tracking.GenerateShipmentCode();

            // Process each cart item using the provided parameters //
            for (int i = 0; i < cartIds.Length; i++)
            {
                var cartId = cartIds[i]; // Sort in Array type due to many products per Cart //

                // Retrieve the cart item based on cartId //
                var cartItem = _db.CartDB
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefault(c => c.CartId == cartId); // Check if CartId is matching or not //

                // Check if the cartItem exists //
                if (cartItem == null)
                {
                    return NotFound($"Cart with ID {cartId} not found."); // Show the Information that CartId is not found // 
                }

                // Create a RetainCartItem using the quantities and prices provided //
                var retainCartItem = new RetainCartItem
                {
                    ProductId = cartItem.CartItems.FirstOrDefault()?.ProductId ?? 0,
                    Quantity = quantities[i],
                    UnitPrice = unitPrices[i],
                };

                // Add the retain cart item to the RetainCarts instance //
                retainCart.RetainCartItems.Add(retainCartItem);

                // After Added to RetainCart remove the cartItem to clear the Cart //
                _db.CartDB.Remove(cartItem);
            }

            // Save RetainCart to the database //
            _db.RetaincartsDB.Add(retainCart);
            _db.SaveChanges();

            // Assign ReCartId to each RetainCartItem //
            foreach (var item in retainCart.RetainCartItems)
            {
                // For each Item in RetainCartItems will be assign RetainCartId attribute as same as retaubCart.ReCartId //
                item.RetainCartId = retainCart.ReCartId;
            }

            // Save the updated RetainCartItems //
            _db.SaveChanges();

            // Create tracking //
            var tracking = new Tracking
            {
                ShipmentCode = shipmentCode,
                Status = "Pending",
                RetainCarts = new List<RetainCarts> { retainCart } // Creat a new RetainCarts in Tracking | value to be the same as the value of "retainCart" //
            };

            // Save tracking //
            _db.TrackingDB.Add(tracking);
            _db.SaveChanges();

            // Redirect to TrackingResult with parameter of shipmentCode that we just generate in this Process //
            return RedirectToAction("TrackingResult", new {shipmentCode});
        }
        // ========== MERGE CART ITEMS TO TRACKINGDB ========== //












    }
}
