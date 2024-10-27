using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.ViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                        DiscountAmount = item.DiscountAmount,
                        TotalDiscountedPrice = item.TotalDiscountedPrice,
                    }).ToList(),
                    
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
        public async Task<IActionResult> CreateShipment(int[] cartIds, int[] quantities, float[] unitPrices, float discountAmount, float discountPrice, CartViewModel cartViewModel
            ,string SelectedPaymethod, string creditcardType, string creditcardName, string creditcardNumber, string creditcardExp , string creditcardCvv
            ,string bankAcc, string bankNumber, IFormFile uploadedPic, QRData qrDB) // Expecting discountedPrices as an array
        {
            // Check if any items are selected //
            if (cartIds == null || cartIds.Length == 0)
            {
                return BadRequest("No items selected.");
            }
            if (cartViewModel == null) 
            {
                return BadRequest("paymentViewModel is null");
            }

            // Validate SelectedPaymentMethod
            if (string.IsNullOrEmpty(SelectedPaymethod))
            {
                return BadRequest("Selected payment method is required.");
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
                var cartId = cartIds[i];

                // Retrieve the cart item based on cartId //
                var cartItem = _db.CartDB
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefault(c => c.CartId == cartId); // Check if CartId is matching or not //

                // Check if the cartItem exists //
                if (cartItem == null)
                {
                    return NotFound($"Cart with ID {cartId} not found.");
                }

                // Create a RetainCartItem using the quantities and prices provided //
                var retainCartItem = new RetainCartItem
                {
                    ProductId = cartItem.CartItems.FirstOrDefault()?.ProductId ?? 0,
                    Quantity = quantities[i],
                    UnitPrice = unitPrices[i],

                    DiscountAmount = discountAmount,
                    TotalDiscountedPrice = discountPrice, // Use the corresponding discounted price for each item

                };

                // Add the retain cart item to the RetainCarts instance //
                retainCart.RetainCartItems.Add(retainCartItem);

                // After added to RetainCart, remove the cartItem to clear the Cart //
                _db.CartDB.Remove(cartItem);
            }

            // Save RetainCart to the database //
            _db.RetaincartsDB.Add(retainCart);
            _db.SaveChanges();
       
            // Assign ReCartId to each RetainCartItem //
            foreach (var item in retainCart.RetainCartItems)
            {
                item.RetainCartId = retainCart.ReCartId;
            }

            // Save the updated RetainCartItems //
            _db.SaveChanges();

            // Save UploadedImage as Byte[] //
            if (uploadedPic != null && uploadedPic.Length > 0)
            {
                qrDB.UserUploadedData = await ConvertToBytes(uploadedPic); // ==== NoT YET ==== //
                _db.SaveChanges();
            }

            // Create and save PaymentMethod
            var paymentMethod = new PaymentMethod
            {
                PaymentmethodType = SelectedPaymethod,
                BankTransfer = SelectedPaymethod == "Bank transfer" ? new BankTransfer
                {
                    BankAccount = bankAcc,
                    BankNumber = bankNumber,
                } : null,
                CreditCard = SelectedPaymethod == "Credit Card" ? new CreditCard
                {
                    NameOnCard = creditcardName,
                    CardNumber = creditcardNumber,
                    ExpiredDate = creditcardExp,
                    CVV = creditcardCvv,
                } : null,
                QRData = SelectedPaymethod == "Prompt Pay" ? new QRData
                {
                    UserUploadedData = qrDB.UserUploadedData,
                } : null
            };

            // Save PaymentMethod to the database
            await _db.PaymentDB.AddAsync(paymentMethod);
            await _db.SaveChangesAsync();

            // Create tracking //
            var tracking = new Tracking
            {
                ShipmentCode = shipmentCode,
                Status = "Pending",
                RetainCarts = new List<RetainCarts> { retainCart },
                PaymentMethod = paymentMethod,
            };

            // Save tracking //
            _db.TrackingDB.Add(tracking);
            _db.SaveChanges();

            // Redirect to TrackingResult with parameter of shipmentCode that we just generated in this process //
            return RedirectToAction("TrackingResult", new { shipmentCode });
        }

        // ========== MERGE CART ITEMS TO TRACKINGDB ========== //

        private async Task<byte[]> ConvertToBytes(IFormFile file)
        {

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }











    }
}
