using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.ViewModel;
using SelectPdf;

namespace Staychill.Controllers.UserController
{
    public class TrackingController : Controller
    {
        private readonly StaychillDbContext _db;
        private readonly Email.IEmailSender _emailSender;
        public TrackingController(Email.IEmailSender emailSender, StaychillDbContext db)
        {
            _emailSender = emailSender;
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
        public async Task<IActionResult> CreateShipment(int[] cartIds, int[] quantities, float[] unitPrices, float discountAmount, float totalamountbefore, float discountPrice, CartViewModel cartViewModel
            ,string SelectedPaymethod, string creditcardType, string creditcardName, string creditcardNumber, string creditcardExp , string creditcardCvv
            ,string bankAcc, string bankNumber, IFormFile uploadedPic, byte[][] productImgbytes, string[] productName, string[] productColor, string usermail) // the reasoned to use a lot of these parameters is because I can declare CartViewModel and transfer the data to this action //
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

            if (SelectedPaymethod == "Credit Card" && (creditcardType == null || creditcardName == null || creditcardNumber == null || creditcardExp == null || creditcardCvv == null))
            {
                return RedirectToAction("PaymentIndex", "Payment");
            }
            if (SelectedPaymethod == "Bank transfer" && (bankAcc == null || bankNumber == null))
            {
                return RedirectToAction("PaymentIndex", "Payment");
            }
            if (SelectedPaymethod == "Prompt Pay" && uploadedPic == null) 
            {
                return RedirectToAction("PaymentIndex","Payment");
            }

            var currentUser = User.Identity.Name; // Define the user account //

            // Create a new RetainCarts variable to store RetainCartItems
            var retainCart = new RetainCarts
            {
                Username = currentUser,
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
                    Color = productColor[i],
                    Quantity = quantities[i],
                    UnitPrice = unitPrices[i],
                    TotalPrice = unitPrices[i] * quantities[i],
                    ProductIMG = productImgbytes[i],
                    DiscountAmount = discountAmount, // discount amount for total price of the products not each item discount amount //
                    TotalDiscountedPrice = discountPrice, // Sum of the total price of products subtract by the discountamount //
                    TotalAmount = totalamountbefore,
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
                    CardType = creditcardType,
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

            // Generate the PDF
            string html = GeneratePdfHtml(shipmentCode, retainCart, paymentMethod); // Use a method to generate the HTML with the cart data
            HtmlToPdf oHtmlToPdf = new HtmlToPdf();
            PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();

            // Create tracking //
            var tracking = new Tracking
            {
                ShipmentCode = shipmentCode, // shipmentCode that we created early //
                Status = "Pending", // First Status after done transaction with website //
                RetainCarts = new List<RetainCarts> { retainCart }, // Assign List of RetainCarts to hold the values of retainCart to make Tracking can be associated with //
                PaymentMethod = paymentMethod, // Assign PaymentMethod inside Tracking to hold the values of paymentMethod that we just create to make Tracking can be associated with //
                Invoice = pdf,
            };

            // Save tracking //
            _db.TrackingDB.Add(tracking);
            _db.SaveChanges();

            var receiver = usermail; // Replace with the user's email
            var subject = "Receipt from Staychill";
            var message = "Here is the receipt for your transaction with Staychill Shop.";

            await _emailSender.SendEmailWithAttachmentAsync(receiver, subject, message, pdf, "Receipt.pdf");

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


        // ========== PDF Formatting ========== //
        private string GeneratePdfHtml(string shipmentCode, RetainCarts retainCart, PaymentMethod paymentMethod)
        {
            string shipmentcode = shipmentCode; // Get the shipment code
            string currentDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            var totalamount = retainCart.RetainCartItems.FirstOrDefault()?.TotalAmount.ToString("N2");
            var discountAmount = retainCart.RetainCartItems.FirstOrDefault()?.DiscountAmount.ToString("N2");
            var totalPrice = retainCart.RetainCartItems.FirstOrDefault()?.TotalDiscountedPrice.ToString("N2");

            var currentUser = User.Identity.Name;
            var existinguser = _db.UserDB.Include(u => u.Address).FirstOrDefault(u => u.Username == currentUser);

            var name = existinguser.Firstname;
            var surname = existinguser.Lastname;

            var housenumber = existinguser.Address.Housenumber;
            var alley = existinguser.Address.Alley;
            var road = existinguser.Address.Road;
            var subdistrict = existinguser.Address.Subdistrict;
            var district = existinguser.Address.District;
            var province = existinguser.Address.Province;
            var country = existinguser.Address.Country;
            var zipcode = existinguser.Address.Zipcode;

            var tel = existinguser.Phonenumber; 


            string html = $@"
            <div style=""padding: 10px 30px 10px 30px;  box-sizing: border-box;"">
                <div style=""width:100%; padding-top:20px; text-align:center;"">
                    <div style=""font-size:3.5rem;"">Staychill-Invoice</div>
                    <div style=""display:flex; justify-content:space-between; align-items:center; font-size:1.2rem;"">
                        <div>Date: {currentDateTime}</div>
                        <div>Order Id: {shipmentCode}</div>
                    </div>
                    <hr />
                    <div style=""width:100%; display:flex; justify-content:space-between; text-align:start"">
                        <div style=""width:45%;"">
                            <div style=""font-size:1.5rem; font-weight:bold; margin-bottom:5px; padding:5px;"">From:</div>
                            <div style=""margin-left:10px; font-size:1.1rem;"">
                                <div style=""font-size:1.3rem; margin-bottom:5px;"">Staychill Shop</div>
                                <div style=""margin-bottom:5px;"">King Mongkut’s University of Technology Thonburi</div>
                                <div style=""margin-bottom:5px;"">126 Pracha Uthit Rd., Bang Mod, Thung Khru, Bangkok 10140. Thailand</div>
                                <div style=""margin-bottom:5px;"">Tel : 012345678</div>
                            </div>
                        </div>
                        <div style=""width:45%; text-align:end;"">
                            <div style=""font-size:1.5rem; font-weight:bold; margin-bottom:5px; padding:5px;"">To:</div>
                            <div style=""margin-right:10px; font-size:1.1rem;"">
                                <div style=""font-size:1.3rem; margin-bottom:5px;"">{name}<span> </span>{surname}</div>
                                <div style=""margin-bottom:5px;"">
                                    <div style=""margin-bottom:5px;"">{housenumber}, {alley}, {road},</div>
                                    <div style=""margin-bottom:5px;"">{subdistrict}, {district}, {province},</div>
                                    <div style=""margin-bottom:5px;"">{zipcode}, {country}</div>
                                </div>
                                <div style=""margin-bottom:5px;"">Tel : {tel}</div>
                            </div>
                        </div>
                    </div>
                    <table style=""width:100%; border-collapse:collapse; text-align:center; margin-top:15px;"">
                        <thead>
                            <tr style=""font-size:1.2rem;"">
                                <th style=""border:1px solid #b7b7b7; background:#b7b7b7; padding:15px;"">Name</th>
                                <th style=""border:1px solid #b7b7b7; background:#b7b7b7; padding:15px;"">Color</th>
                                <th style=""border:1px solid #b7b7b7; background:#b7b7b7; padding:15px;"">Quantity</th>
                                <th style=""border:1px solid #b7b7b7; background:#b7b7b7; padding:15px;"">Unit Price</th>
                                <th style=""border:1px solid #b7b7b7; background:#b7b7b7; padding:15px;"">Price</th>
                            </tr>
                        </thead>
                        <tbody>";

                        foreach (var item in retainCart.RetainCartItems)
                        {
                            html += $@"
                            <tr>
                                <td style=""border:1px solid #b7b7b7; padding:15px; text-align:start;"">{item.ProductName}</td>
                                <td style=""border:1px solid #b7b7b7; padding:15px; text-align:start;"">{item.Color}</td>
                                <td style=""border:1px solid #b7b7b7; padding:15px;"">{item.Quantity}</td>
                                <td style=""border:1px solid #b7b7b7; padding:15px; text-align:end;"">{item.UnitPrice}</td>
                                <td style=""border:1px solid #b7b7b7; padding:15px; text-align:end;"">{item.TotalPrice}</td>
                            </tr>";
                        }

                        // Add the Total row here, after the foreach loop
                        html += $@"
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style=""border:1px solid #b7b7b7; background:#ebebeb; padding:10px; padding-right:15px; font-size:1rem; text-align:start; font-weight:bold;"">
                                    Subtotal
                                </td>
                                <td style=""border:1px solid #b7b7b7; background:#ebebeb; padding:10px; padding-right:15px; font-size:1rem; text-align:end;"">
                                    <div style=""display:flex; justify-content:space-between; align-items:center;"">
                                        <div>฿</div>
                                        <div>{totalamount}</div>
                                    </div>
                                </td>
                            </tr>";
                        html += $@"
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style=""border:1px solid #b7b7b7; background:#ebebeb; padding:10px; padding-right:15px; font-size:1rem; text-align:start; font-weight:bold;"">
                                    Discount
                                </td>
                                <td style=""border:1px solid #b7b7b7; background:#ebebeb; padding:10px; padding-right:15px; font-size:1rem; text-align:end;"">
                                    <div style=""display:flex; justify-content:space-between; align-items:center;"">
                                        <div>฿</div>
                                        <div>-{discountAmount}</div>
                                    </div>
                                </td>
                            </tr>";

                        html += $@"
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style=""border:1px solid #b7b7b7; background:#ebebeb; padding:10px; padding-right:15px; font-size:1rem; text-align:start; font-weight:bold;"">
                                    Total
                                </td>
                                <td style=""border:1px solid #b7b7b7; background:#ebebeb; padding:10px; padding-right:15px; font-size:1rem; text-align:end; font-weight:bold"">
                                    <div style=""display:flex; justify-content:space-between; align-items:center;"">
                                        <div>฿</div>
                                        <div>{totalPrice}</div>
                                    </div>
                                </td>
                            </tr>";

                    html += $@"
                        </tbody>
                    </table>
                    <div style=""text-align:end; font-size:1.2rem; margin-top:15px;"">
                        <div><span style=""font-weight:bold;"">Payment method:</span> {paymentMethod.PaymentmethodType}</div>
                    </div>
                    <hr style=""margin-top:10px;""/>
                    <div style=""font-size:3rem; font-weight:bold; margin-top:35px;"">THANK YOU.</div>
                </div>";

            return html;
        }
        // ========== PDF Formatting ========== //

    }
}
