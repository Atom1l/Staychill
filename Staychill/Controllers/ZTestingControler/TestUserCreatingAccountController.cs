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

namespace Staychill.Controllers.ZTestingControler
{
    public class TestUserCreatingAccountController : Controller
    {
        private readonly StaychillDbContext _db;

        public TestUserCreatingAccountController(StaychillDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //-------------------------- USER SECTION -------------------------- //
                // CONVERTTOBYTES //
                private async Task<byte[]> ConvertToBytes(IFormFile file)
                {
                    
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }

                //-------------------------- (INDEX) Display the Data of User in form of table --------------------------//
                public IActionResult UserIndex()
                {
                    var user = _db.UserDB.Include(u => u.Address).ToList(); // Combine Address attributes to UserDB //

                    // Get the Database data to ViewModel attributes and Display in View //
                    var UserViewModel = user.Select(u => new UserViewModel
                    {
                        Id = u.Id, // Ensure Id is included for Edit/Delete links
                        // User Info //
                        Firstname = u.Firstname,
                        Lastname = u.Lastname,
                        Username = u.Username,
                        Email = u.Email,
                        PhoneNumber = u.Phonenumber,
                        Password = u.Password,

                        // Address Info //
                        HouseNumber = u.Address?.Housenumber,
                        Alley = u.Address?.Alley,
                        Road = u.Address?.Road,
                        Subdistrict = u.Address?.Subdistrict,
                        District = u.Address?.District,
                        Province = u.Address?.Province,
                        Country = u.Address?.Country,
                        ZipCode = u.Address?.Zipcode

                    }).ToList(); // Change the result of IEnumerable<> into List<> // 

                    return View(UserViewModel);
                }


                //-------------------------- (CREATE) --------------------------//
                public IActionResult UserCreate()
                {
                    return View(new UserViewModel());
                }

                // POST:(CREATE) //
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> UserCreate(UserViewModel userViewModel) // userViewModel as a parameter of UserViewModel work as a representing //
                {
                    if (ModelState.IsValid) // Check ModelState if it valid or not then proceed to the condition //
                    {
                        if (_db.UserDB.Any(u => u.Username == userViewModel.Username || u.Email == userViewModel.Email))
                        {// Check if there are any Username or Email that already exist in database //
                            ModelState.AddModelError(string.Empty, "Username or Email already exist");
                            return View(userViewModel);
                        }

                        User newUser = new User() // Creating new object in User.cs with the new variable newUser //
                        {
                            // User Info //
                            Firstname = userViewModel.Firstname,
                            Lastname = userViewModel.Lastname,
                            Username = userViewModel.Username,
                            Email = userViewModel.Email,
                            Phonenumber = userViewModel.PhoneNumber,
                            Password = userViewModel.Password,

                            // Address Info //
                            Address = new Address
                            {
                                Housenumber = userViewModel.HouseNumber,
                                Alley = userViewModel.Alley,
                                Road = userViewModel.Road,
                                Subdistrict = userViewModel.Subdistrict,
                                District = userViewModel.District,
                                Province = userViewModel.Province,
                                Country = userViewModel.Country,
                                Zipcode = userViewModel.ZipCode
                            }
                        };

                        _db.UserDB.Add(newUser); // at UserDB will add newUser data into its database //
                        await _db.SaveChangesAsync(); // Save the new data into database //

                        return RedirectToAction("UserIndex"); // Return to Action name "UserIndex" //
                    }

                    return View(userViewModel);
                }

                //-------------------------- GET: TestUserCreatingAccount/Delete/5 --------------------------//
                public async Task<IActionResult> UserDelete(int? id)
                {
                    if (id == null) // If Id = nothing then NotFound //
                    {
                        return NotFound();
                    }

                    // Include the Address.cs into UserDB to combine attributes | then check for database Id by using parameter id that we mentioned to compare //
                    var user = await _db.UserDB.Include(u=>u.Address).FirstOrDefaultAsync(m => m.Id == id);
                    if (user == null)
                    {// If found nothing then return NotFound //
                        return NotFound();
                    }

                    var userViewModel = new UserViewModel // Creating new object in UserViewModel.cs with the new variable userViewModel //
                    {
                        // User Info //
                        Id = user.Id,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Username = user.Username,
                        Email = user.Email,
                        PhoneNumber = user.Phonenumber,
                        Password = user.Password,

                        // Address Info //
                        HouseNumber = user.Address?.Housenumber,
                        Alley = user.Address?.Alley,
                        Road = user.Address?.Road,
                        Subdistrict = user.Address?.Subdistrict,
                        District = user.Address?.District,
                        Province = user.Address?.Province,
                        Country = user.Address?.Country,
                        ZipCode = user.Address?.Zipcode
                    };

                    return View(userViewModel);
                }

                //-------------------------- POST: TestUserCreatingAccount/Delete/5 --------------------------//
                [HttpPost, ActionName("UserDelete")] // Can call by "Post" or ActionName "UserDelete" //
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DeleteConfirmed(int id)
                {
                    var user = await _db.UserDB.FindAsync(id); // Wait for UserDB to Find Id that matching id //
                    if (user != null) // If not null then proceed into condition //
                    {
                        _db.UserDB.Remove(user); // Remove user.cs with the same id as we used in UserDB //
                        await _db.SaveChangesAsync(); // Save Change of deleted //
                        return RedirectToAction(nameof(UserIndex)); // Return to UserIndex //
                    }

                    return NotFound(); // If id cannot be found then return NotFound //
                }

                private bool UserViewModelExists(int id) // Check if parameter id is matching with Id in UserViewModel and return in 1 or 0 //
                {
                    return _db.UserViewModel.Any(e => e.Id == id);
                }

        //-------------------------- USER SECTION -------------------------- //

        //-------------------------- PRODUCT SECTION -------------------------- //

            //-------------------------- (INDEX) --------------------------//
                public async Task<IActionResult> ProductIndex()
                {
                    var model = await _db.ProductDB.Include(p => p.Images).ToListAsync(); // Include Images from productImage to ProductDB //
                    return View(model);
                }

                //-------------------------- (CREATE) --------------------------//
                public IActionResult ProductCreate()
                {
                    var product = new Product(); // make a new object name product using attributes from Product.cs //
                    return View(product);
                }

                // POST:(CREATE) //
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> ProductCreate(Product product, IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4)
                {
                    if (ModelState.IsValid)
                    {
                        _db.ProductDB.Add(product); // Save the data from product that have Product.cs attributes //
                        await _db.SaveChangesAsync();

                        var productImages = new ProductImages // Convert IMG to Bytes to save in database //
                        {
                            ProductId = product.Id,
                            Image1 = await ConvertToBytes(image1),
                            Image2 = await ConvertToBytes(image2),
                            Image3 = await ConvertToBytes(image3),
                            Image4 = await ConvertToBytes(image4),
                        };

                        _db.ProductImagesDB.Add(productImages); // Save the byte type of data into database //
                        await _db.SaveChangesAsync();

                        return RedirectToAction("ProductIndex");
                    }
                    return View("Index_Product");
                }


                //-------------------------- (DELETE) --------------------------//
                public async Task<IActionResult> ProductDelete( int id)
                {
                    // Find ProductDB include ProductImages Id if matching with id //
                    var product = await _db.ProductDB.Include(m => m.Images).FirstOrDefaultAsync(p => p.Id == id); 
                    if(product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }

                // POST:(DELETE) //
                [HttpPost, ActionName("ProductDelete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> ProductDeleteConfirmed(int id)
                {
                    var product = await _db.ProductDB.FindAsync(id); // If match and confirmed then proceed to delete the data that contain inside int id //
                    if(product != null)
                    {
                        _db.ProductDB.Remove(product);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("ProductIndex");
                    }
                    return NotFound();
        }

                //-------------------------- (CART) --------------------------//
                // CART Controller
                public IActionResult ProductAddCart()
                {
                    var cartItems = _db.CartDB
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product) // Load the Product for each CartItem
                        .ToList();
    
                    var viewModel = new TrackingViewModel
                    {
                        CartItemDetails = cartItems,
                        TotalAmount = cartItems.SelectMany(c => c.CartItems).Sum(item => item.TotalPrice) // Calculate total from all CartItems
                    };

                    return View(viewModel);
                }


                [HttpPost]
                [Route("ProductAddToCart")]
                public IActionResult ProductAddToCart(int productId, int quantity) 
                {
                    var product = _db.ProductDB.FirstOrDefault(p => p.Id == productId); // Check if productId matches with ProductDB.Id
                    if (product == null) 
                    {
                        return Json(new { success = false, message = "Product not found" }); // Return as JSON
                    }

                    // Find the existing Cart (assuming there's only one cart on the website)
                    var cart = _db.CartDB.Include(c => c.CartItems)
                                          .FirstOrDefault(); // Get the first Cart (since there is only one cart)
    
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

                    return Json(new { success = true, message = "Product added to cart" }); // Return success response as JSON
                }


        // POST:(CART.DELETE) //
        [HttpPost]
        public IActionResult ProductRemoveCart(int RemovecartId, int[] RemoveitemId)
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

            return RedirectToAction("ProductAddCart"); // Redirect to the appropriate action
        }
        //-------------------------- (DETAILS) --------------------------//
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _db.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id); // Load Product with Images
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //-------------------------- (EDIT - GET) --------------------------//
        public async Task<IActionResult> ProductEdit(int id)
        {
            var product = await _db.ProductDB.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id); // Load Product with Images
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //-------------------------- (EDIT - POST) --------------------------//
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.ProductDB.Update(updatedProduct);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(ProductIndex));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(updatedProduct);
        }

        // Check if Product exists
        private bool ProductExists(int id)
        {
            return _db.ProductDB.Any(e => e.Id == id);
        }







        //-------------------------- PRODUCT SECTION -------------------------- //

        //-------------------------- PAYMENT SECTION -------------------------- //

        public IActionResult PaymentIndex()
                {
                    return View();
                }


                public async Task<IActionResult> PaymentBankIndex()
                {
                    var bankAccounts = await _db.BankAccDB.ToListAsync(); // Convert BankAccDB to List //

                    if (bankAccounts == null)
                    {
                        return NotFound(); // Handle the case when there's no data
                    }

                    return View(bankAccounts);
                }

                public async Task<IActionResult> PaymentQRIndex()
                {
                    var qr = await _db.QRDataDB.ToListAsync(); // Convert QRDataDB to List //
                    if (qr == null)
                    {
                        return NotFound();
                    }
                    return View(qr);
                }

                public IActionResult PaymentCardIndex()
                {
                    var produt = _db.CreditCardsDB.ToList(); // Convert CreditCardsDB to List //
                    return View(produt);
                }


                //-------------------------- (CREATE) --------------------------//
                public IActionResult PaymentBankCreate()
                {
                    var bankacc = new BankAccount();
                    return View(bankacc);
                }

                public IActionResult PaymentQRCreate()
                {
                    var qr = new QRData();
                    return View(qr);
                }

                public IActionResult PaymentCardPayment()
                {
                    var card = new CreditCard();
                    return View(card);
                }

                // POST:(CREATE) //
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> PaymentBankCreate(BankAccount bankacc , IFormFile Image)
                {
                    if (ModelState.IsValid)
                    {
                        if (Image != null && Image.Length > 0) // Connvert Bank Account picture that uploaded into byte[] type of file //
                        {
                            bankacc.BankPicsData = await ConvertToBytes(Image);
                        }

                        _db.BankAccDB.Add(bankacc); // Save byte[] into the database //
                        await _db.SaveChangesAsync();

                        return RedirectToAction("PaymentBankIndex"); // Return to Index //
                    }
                    return View(bankacc);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> PaymentQRCreate(QRData qrdata, IFormFile Image)
                {
                    if (ModelState.IsValid)
                    {
                        if(Image != null && Image.Length > 0) // Save the QR picture that uploaded into byte[] type of file //
                        {
                            qrdata.UserUploadedData = await ConvertToBytes(Image);
                        }
                        _db.QRDataDB.Add(qrdata); // Save byte[] into the database //
                        await _db.SaveChangesAsync();
                        return RedirectToAction("PaymentQRIndex"); // Return to Index //
                    }
                    return View(qrdata);
                }

                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> PaymentCardPayment(CreditCard creditcard)
                {
                    if (ModelState.IsValid)
                    {
                        _db.CreditCardsDB.Add(creditcard); // Save the Creditcard data that user fill in the form into database //
                        await _db.SaveChangesAsync();
                        return RedirectToAction("PaymentCardIndex"); // return to Index //
                    }
                    return View(creditcard);
                }

                //-------------------------- (DELETE) --------------------------//
                public async Task<IActionResult> PaymentBankDelete(int id)
                {
                    var bankAcc = await _db.BankAccDB.FirstOrDefaultAsync(p => p.Id == id);
                    if (bankAcc == null)
                    {
                        return NotFound();
                    }
                    return View(bankAcc);
                }

                public async Task<IActionResult> PaymentQRDelete(int id)
                {
                    var qr = await _db.QRDataDB.FirstOrDefaultAsync(q => q.Id == id);
                    if(qr == null)
                    {
                        return NotFound();
                    }
                    return View(qr);
                }

                public async Task<IActionResult> PaymentCardDelete(int id)
                {
                    var card = await _db.CreditCardsDB.FirstOrDefaultAsync(c => c.Id == id);
                    if(card == null)
                    {
                        return NotFound();
                    }
                    return View(card);
                }

                // POST:(DELETE) //
                [HttpPost,ActionName("PaymentBankDelete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> PaymentBankDeleteConfirmed(int id)
                {
                    var bankAcc = await _db.BankAccDB.FindAsync(id);
                    if (bankAcc != null)
                    {
                        _db.BankAccDB.Remove(bankAcc);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("PaymentBankIndex");
                    }
                    return NotFound();
                }

                [HttpPost, ActionName("PaymentQRDelete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> PaymentQRDeleteConfirmed(int id)
                {
                    var qr = await _db.QRDataDB.FindAsync(id);
                    if (qr != null)
                    {
                        _db.QRDataDB.Remove(qr);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("PaymentQRIndex");
                    }
                    return NotFound();
                }

                [HttpPost, ActionName("PaymentCardDelete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> PaymentCardDeleteConfirmed(int id)
                {
                    var card = await _db.CreditCardsDB.FindAsync(id);
                    if(card  != null)
                    {
                        _db.CreditCardsDB.Remove(card);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("PaymentCardIndex");
                    }
                    return NotFound();
                }


                // BANKACCOPTIONS //
                // This controller is about showing the combining between BankAccDB and bankTransfer //
                // To make a virtual transaction for user using bank account and then bank number //
                public async Task<IActionResult> PaymentBankTransferCreate()
                {
                    var bankTransfer = new BankTransfer
                    {
                        Accounts = await _db.BankAccDB.ToListAsync()
                    };

                    return View(bankTransfer);
                }

        //-------------------------- PAYMENT SECTION -------------------------- //


        //-------------------------- DISCOUNT SECTION -------------------------- //

                //-------------------------- (INDEX) --------------------------//
                public IActionResult DiscountIndex()
                {
                    var discount = _db.DiscountDB.ToList(); // Convert DiscountDB to List type and then contain is discount variable //
                    return View(discount); // Show the result //
                }

                //-------------------------- (CREATE) --------------------------//
                public IActionResult DiscountCreate() 
                {
                    var discount = new Discount(); // Make new object name discount in Discount.cs // 
                    return View(discount);        
                }

                // POST:(CREATE) //
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DiscountCreate(Discount model)
                {
                    if (ModelState.IsValid)
                    {
                        _db.DiscountDB.Add(model);  // Add model that is data that user fill through form as a new data in DiscountDB //
                        await _db.SaveChangesAsync(); // Save Changes //

                        return RedirectToAction("DiscountIndex"); // Return to DiscountIndex //

                    }
                    return View(model);
                }

                //-------------------------- (DELETE) --------------------------//
                public async Task<IActionResult> DiscountDelete(int id)
                {
                    // check for database Id by using parameter id that we mentioned to compare //
                    var user = await _db.DiscountDB.FirstOrDefaultAsync(m => m.Id == id);
                    if (user == null)
                    {// If found nothing then return NotFound //
                        return NotFound();
                    }

                    return View(user);
                }

                // POST:(DELETE) //
                [HttpPost, ActionName("DiscountDelete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> DiscountDeleteConfirmed(int id)
                {
                    var discount = await _db.DiscountDB.FirstOrDefaultAsync(d => d.Id == id); // Wait for DiscountDB to find if id is matching DiscountDB.Id //
                    if(discount != null) // If id was found the proceed to the condition //
                    {
                        _db.DiscountDB.Remove(discount); // Remove the DiscountDb item that have id the same with discount variable //
                        await _db.SaveChangesAsync(); // Save Changes //
                    }
                    return RedirectToAction("DiscountIndex"); // Return to DiscountIndex //
                }

                //-------------------------- (CALCULATE EXAMPLE) --------------------------//
                public IActionResult DiscountCalculate(int id)
                {
                    // Fetch the discount from the database using the ID
                    var discount = _db.DiscountDB.FirstOrDefault(d => d.Id == id); //Find the matching between id in cshtml and Id in DiscountDB //
                    if (discount == null)
                    {
                        return NotFound(); // If not foudn then return NotFound //
                    }

                    ViewBag.Discount = discount; // Pass the discount to the view //
                    return View();
                }

                // POST:(CALCULATE) //
                [HttpPost]
                [ValidateAntiForgeryToken]
                public IActionResult CalculateDiscount(decimal originalPrice, int discountId) // Original price is new input that was created in this cshtml //
                {
   
                    var discount = _db.DiscountDB.FirstOrDefault(d => d.Id == discountId); // Find if DiscountDB.Id was matching with discountId //
                    if (discount == null)
                    {
                        return NotFound(); // If not matching the return NotFound //
                    }

                    // Calculate the discount amount based on the discount percentage
                    decimal discountAmount = originalPrice * (discount.DiscountAmount / 100.0m); // Ex. 100*(80/100) = 80 //
                    decimal discountedPrice = originalPrice - discountAmount; // Ex. 100-80 = 20 //

                    // Pass the results to the view //
                    ViewBag.OriginalPrice = originalPrice; // The Price that we fill in form //
                    ViewBag.DiscountAmount = discount.DiscountAmount; // this is DiscountAmount that DiscountDB given //
                    ViewBag.DiscountedPrice = discountedPrice; // Result of the calculate price //

                    return View("DiscountCalculateResult"); // Return to DiscountCalculateResult //
                }

        //-------------------------- DISCOUNT SECTION -------------------------- //


        //-------------------------- TRACKING SECTION -------------------------- //

                //-------------------------- (INDEX) Display the Data of ShipmentCode in form of table --------------------------//
                public IActionResult TrackingIndex()
                {
                    var tracklist = _db.TrackingDB.ToList(); // Create a tracklist variable to contain TrackingDB database that convert to list type //
                    return View(tracklist); // Return with the data of tracklist(TrackingDB.Tolist) //
                }

                //-------------------------- (CREATE) --------------------------//
                [HttpGet]
                public IActionResult TrackingGenerateCode() // Create a code and choose its status //
                {
                    var model = new Tracking(); // Create a model to receive a new data that have attributes from Tracking.cs //
                    return View(model); // Return with the data of model (new Tracking()) //
                }

                // POST:(CREATE) //
                [HttpPost]
                public async Task<IActionResult> TrackingGenerateCode(Tracking model)
                {
                    if (ModelState.IsValid) // Check the modelstate if it valid or not //
                    {
                        // Create a variable to contain the GenerateShipmentCode() //
                        var generatedCode = Tracking.GenerateShipmentCode(); // GenerateShipmentCode output is string(Shipment) and We set that value to generatedCode variable //
                        model.ShipmentCode = generatedCode; // (model is a parameter of Tracking) We refer ShipmentCode to set it value to generatedCode //

                        var tracking = new Tracking // Set the constructor to use the Tracking.cs attributes values //
                        {
                            ShipmentCode = model.ShipmentCode, // ShipmentCode will be model.ShipmentCode //
                            Status = model.Status // Status wil be model.Status //
                        };

                        _db.TrackingDB.Add(tracking); // Use that constructor to make TrackingDB add the value to its database //
                        await _db.SaveChangesAsync(); // Save Changes (The new trackingcode we added) //

                        return RedirectToAction("TrackingIndex"); // Return to TrackingIndex //
                    }
                    return View(model); // If modelstate is not valid //
                }

                //-------------------------- (EDIT) --------------------------//

                public async Task<IActionResult> TrackingEdit(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var tracking = await _db.TrackingDB.FindAsync(id);
                    if (tracking == null)
                    {
                        return NotFound();
                    }

                    return View(tracking); // Return the existing tracking data to the Edit view
                }

                // POST: Edit
                [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> TrackingEdit(int id, Tracking model)
                {
                    if (id != model.Id) // Check if the id in the route matches the model id
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid) // Validate the model
                    {
                        try
                        {
                            _db.Update(model); // Update the tracking object in the database
                            await _db.SaveChangesAsync(); // Save changes
                        }
                        catch (DbUpdateConcurrencyException) // Handle concurrency issues
                        {
                            if (!TrackingExists(model.Id))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        return RedirectToAction(nameof(TrackingIndex)); // Redirect to the index after editing
                    }
                    return View(model); // Return the model back to the view if the model state is invalid
                }

                // Helper method to check if the tracking exists
                private bool TrackingExists(int id)
                {
                    return _db.TrackingDB.Any(e => e.Id == id);
                }



                //-------------------------- (EDIT) --------------------------//

                //-------------------------- (DELETE) --------------------------//
                public async Task<IActionResult> TrackingDelete(int? id)
                {
                    if (id == null) // If Id = nothing then NotFound //
                    {
                        return NotFound();
                    }

                    // check for database Id by using parameter id that we mentioned to compare //
                    var user = await _db.TrackingDB.FirstOrDefaultAsync(m => m.Id == id);
                    if (user == null)
                    {// If found nothing then return NotFound //
                        return NotFound();
                    }

                    return View(user);
                }
                // POST:(DELETE) //
                [HttpPost, ActionName("TrackingDelete")]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> TrackingDeleteConfirmed(int id)
                {
                    var tracking = await _db.TrackingDB.FindAsync(id); // wait TrackingDB to find matching Id and id //
                    if (tracking != null)
                    {
                        _db.TrackingDB.Remove(tracking); // If id was matching then Remove this item from database //
                        await _db.SaveChangesAsync(); // Save Changes //
                    }
                    return RedirectToAction("TrackingIndex");
                }

        //-------------------------- TRACKING SECTION -------------------------- //
    }
}
