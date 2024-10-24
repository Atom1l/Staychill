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
            // Retrieve the tracking entry based on the shipment code
            var trackingEntry = _db.TrackingDB
                .Include(t => t.RetainCarts)
                .ThenInclude(rc => rc.RetainCartItems)
                .FirstOrDefault(t => t.ShipmentCode == shipmentCode);

            // Check if tracking entry exists
            if (trackingEntry == null)
            {
                return NotFound($"Tracking information for shipment code {shipmentCode} not found.");
            }

            // Map the data to the ViewModel
            var viewModel = new TrackingResultViewModel
            {
                ShipmentCode = trackingEntry.ShipmentCode,
                Status = trackingEntry.Status,
                RetainCartItems = trackingEntry.RetainCarts.SelectMany(rc => rc.RetainCartItems)
                    .Select(item => new RetainCartItemViewModel
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    }).ToList()
            };

            // Return the view with the ViewModel
            return View(viewModel);
        }


        public IActionResult TrackingIndex()
        {
            return View();
        }

        // ========== Merge CartItems to TrackingDB ========== //
        [HttpPost]
        public IActionResult CreateShipment(int[] cartIds, int[] quantities, float[] unitPrices, float[] totalPrices)
        {
            // Check if any items are selected
            if (cartIds == null || cartIds.Length == 0)
            {
                return BadRequest("No items selected.");
            }

            // Create a new RetainCarts instance to store retained cart items
            var retainCart = new RetainCarts
            {
                RetainCartItems = new List<RetainCartItem>()
            };

            // Generate shipment code
            string shipmentCode = Tracking.GenerateShipmentCode();

            // Process each cart item using the provided parameters
            for (int i = 0; i < cartIds.Length; i++)
            {
                var cartId = cartIds[i];

                // Retrieve the cart item based on cartId
                var cartItem = _db.CartDB
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefault(c => c.CartId == cartId);

                // Check if the cart item exists
                if (cartItem == null)
                {
                    return NotFound($"Cart with ID {cartId} not found.");
                }

                // Create a RetainCartItem using the quantities and prices provided
                var retainCartItem = new RetainCartItem
                {
                    ProductId = cartItem.CartItems.FirstOrDefault()?.ProductId ?? 0, // Assuming you want the first product's ID
                    Quantity = quantities[i],
                    UnitPrice = unitPrices[i],
                };

                // Add the retain cart item to the RetainCarts instance
                retainCart.RetainCartItems.Add(retainCartItem);

                // Optionally, remove the cart item from the CartDB if necessary
                _db.CartDB.Remove(cartItem);
            }

            // Save RetainCart to the database
            _db.RetaincartsDB.Add(retainCart);
            _db.SaveChanges(); // This will generate ReCartId

            // Assign ReCartId to each RetainCartItem
            foreach (var item in retainCart.RetainCartItems)
            {
                item.RetainCartId = retainCart.ReCartId;
            }

            // Save the updated RetainCartItems
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

            // Redirect or return a view after processing
            return RedirectToAction("TrackingResult", new {shipmentCode});
        }











    }
}
