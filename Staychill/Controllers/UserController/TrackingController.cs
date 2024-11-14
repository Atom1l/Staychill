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
        public IActionResult TrackingResult(string shipmentCode= "WA45WbBt") // Display an items and it status after User fill the input in TrackingIndex or Clicked Payment //
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
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        DiscountAmount = item.DiscountAmount,
                        TotalDiscountedPrice = item.TotalDiscountedPrice,
                        ProductIMG = item.ProductIMG,
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
            ,string bankAcc, string bankNumber, IFormFile uploadedPic, byte[] productImgbytes, string[] productName) // the reasoned to use a lot of these parameters is because I can declare CartViewModel and transfer the data to this action //
        {
            // Check if any items are selected //
            if (cartIds == null || cartIds.Length == 0)
            {
                return BadRequest("No items selected.");
            }
            // Check if the payment value has been fullfill yet //
            if (cartViewModel == null) 
            {
                return BadRequest("paymentViewModel is null");
            }
            // Check the status of SelectedPaymentMethod if its null or not //
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
                var cartId = cartIds[i]; // make each Id inside the array of cartIds contain into cartId to procress each product one by one  //

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
                    ProductName = productName[i],
                    Quantity = quantities[i],
                    UnitPrice = unitPrices[i],
                    ProductIMG = productImgbytes,
                    DiscountAmount = discountAmount, // discount amount for total price of the products not each item discount amount //
                    TotalDiscountedPrice = discountPrice, // Sum of the total price of products subtract by the discountamount //

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

            // Create and save PaymentMethod //
            var paymentMethod = new PaymentMethod
            {
                PaymentmethodType = SelectedPaymethod, // Ex. "Bank Transfer", "Credit Card", "Prompt Pay" //
                BankTransfer = SelectedPaymethod == "Bank transfer" ? new BankTransfer // Create a new data inside Banktransfer database //
                {
                    BankAccount = bankAcc, // Ex. "KrungSri", "KasiKorn" //
                    BankNumber = bankNumber,
                } : null,
                CreditCard = SelectedPaymethod == "Credit Card" ? new CreditCard // Create a new data inside CreditCard database //
                {
                    NameOnCard = creditcardName, // Name on card //
                    CardNumber = creditcardNumber,
                    ExpiredDate = creditcardExp,
                    CVV = creditcardCvv,
                } : null,
                QRData = SelectedPaymethod == "Prompt Pay" && uploadedPic != null && uploadedPic.Length > 0 ? new QRData // Create a new data inside QRData database //
                {
                    UserUploadedData = await ConvertToBytes(uploadedPic), // Make UserUploadeddata to contain the image evidence of the transaction by convert in to byte[] type //
                } : null
            };

            // Save PaymentMethod to the database
            await _db.PaymentDB.AddAsync(paymentMethod);
            await _db.SaveChangesAsync();

            // Create tracking //
            var tracking = new Tracking
            {
                ShipmentCode = shipmentCode, // shipmentCode that we created early //
                Status = "Pending", // First Status after done transaction with website //
                RetainCarts = new List<RetainCarts> { retainCart }, // Assign List of RetainCarts to hold the values of retainCart to make Tracking can be associated with //
                PaymentMethod = paymentMethod, // Assign PaymentMethod inside Tracking to hold the values of paymentMethod that we just create to make Tracking can be associated with //
            };

            // Save tracking //
            _db.TrackingDB.Add(tracking);
            _db.SaveChanges();

            // Redirect to TrackingResult with parameter of shipmentCode that we just generated in this process to see the tracking result //
            return RedirectToAction("TrackingResult", new { shipmentCode });
        }
        // ========== MERGE CART ITEMS TO TRACKINGDB ========== //

        // ========== Convert Image into Byte to keep in database ========== //
        private async Task<byte[]> ConvertToBytes(IFormFile file) // Retrieve a parameter as a file type //
        {

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        // ========== Convert Image into Byte to keep in database ========== //











    }
}
