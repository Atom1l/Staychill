using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.ViewModel;
using SelectPdf;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Staychill.Models.ProductModel.DiscountModel;

namespace Staychill.Controllers.AdminController
{
    public class AdminController : Controller
    {
        private readonly StaychillDbContext _db;
        public AdminController(StaychillDbContext db)
        {
            _db = db;
        }
        

        // ============================================= Discount ============================================= //
        public IActionResult Discount(string discountquery) // Admin-Discount-Index //
        {
            if (string.IsNullOrEmpty(discountquery))
            {
                return View("Discount", _db.DiscountDB.ToList()); // Return all movies if the query is null or empty
            }
            else
            {
                string normalizedQuery = discountquery.Replace("%", "").Trim();

                if (normalizedQuery == "0") // To prevent searching 0% and the other like 80%,20% is showing //
                {
                    return View("Discount", new List<Discount>());
                }

                var discountlist = _db.DiscountDB.ToList();
                var filtereddiscountCode = discountlist.Where(
                    discountCode => discountCode.DiscountCode.Contains(normalizedQuery) ||
                    discountCode.DiscountAmount.ToString() == normalizedQuery
                ).ToList();
                return View("Discount", filtereddiscountCode);
            }
        }

        [HttpGet]
        public IActionResult DiscountCreate() // Admin-Discount-Create(GET) //
        {
            var discount = new Discount();
            return View(discount);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiscountCreate(Discount model) // Admin-Discount-Create(POST) //
        {
            if (ModelState.IsValid)
            {
                _db.DiscountDB.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Discount");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult DiscountEdit(int id) // Admin-Discount-Edit(GET) //
        {
            var discountId = _db.DiscountDB.FirstOrDefault(d => d.Id == id);
            if (discountId == null)
            {
                return RedirectToAction("Discount");
            }
            return View(discountId);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DiscountEdit(Discount editmodel) // Admin-Discount-Edit(POST) //
        {
            if (ModelState.IsValid)
            {
                var existingDiscount = _db.DiscountDB.FirstOrDefault(d => d.Id == editmodel.Id);
                if (existingDiscount == null)
                {
                    return RedirectToAction("Discount");
                }
                existingDiscount.DiscountCode = editmodel.DiscountCode;
                existingDiscount.DiscountAmount = editmodel.DiscountAmount;
                await _db.SaveChangesAsync();
                return RedirectToAction("Discount");
            }
            return View(editmodel);
        }

        public IActionResult DiscountDelete(int id) // Admin-Discount-Delete // 
        {
            var existingDiscount = _db.DiscountDB.FirstOrDefault(d => d.Id == id);
            if (existingDiscount == null)
            {
                return RedirectToAction("Discount");
            }
            _db.DiscountDB.Remove(existingDiscount);
            _db.SaveChanges();
            return RedirectToAction("Discount");
        }
        // ============================================= Discount ============================================= //


        // ============================================= Tracking ============================================= //
        public IActionResult Tracking(string trackingquery) // Admin-Tracking-Index //
        {
            if (string.IsNullOrEmpty(trackingquery))
            {
                return View("Tracking", _db.TrackingDB.Include(t => t.PaymentMethod).ToList());
            }
            else
            {
                var tracking = _db.TrackingDB.Include(t => t.PaymentMethod).ToList();
                var filteredTracking = tracking.Where(tracking => tracking.ShipmentCode.Contains(trackingquery) ||
                tracking.Status.Contains(trackingquery)).ToList();
                return View("Tracking", filteredTracking);
            }
        }

        [HttpGet]
        public IActionResult TrackingEdit(string shipmentCode) // Admin-Tracking-Edit(GET) //
        {
            var existingTracking = _db.TrackingDB.FirstOrDefault(t => t.ShipmentCode == shipmentCode);
            if (existingTracking == null)
            {
                return RedirectToAction("Tracking");
            }
            return View(existingTracking);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrackingEdit(Tracking editTracking) // Admin-Tracking-Edit(POST) //
        {
            var existingTracking = _db.TrackingDB.FirstOrDefault(t => t.ShipmentCode == editTracking.ShipmentCode);
            if (existingTracking == null)
            {
                return RedirectToAction("Tracking");
            }
            existingTracking.Status = editTracking.Status;
            await _db.SaveChangesAsync();
            return RedirectToAction("Tracking");
        }
        public IActionResult TrackingDetail(int paymentmethodId) // Admin-Tracking-Detail //
        {
            var tracking = _db.TrackingDB.Include(t => t.PaymentMethod)
                .ThenInclude(tc => tc.CreditCard).Include(t => t.PaymentMethod).ThenInclude(tb => tb.BankTransfer)
                .Include(t => t.PaymentMethod).ThenInclude(tq => tq.QRData).FirstOrDefault(t => t.PaymentMethod.PaymentmethodId == paymentmethodId);
            if (tracking == null)
            {
                return RedirectToAction("Payment");
            }
            tracking.RetainCarts = _db.RetaincartsDB
            .Where(rc => rc.Tracking.PaymentMethod.PaymentmethodId == paymentmethodId)
            .Include(rc => rc.RetainCartItems)
            .ToList();
            return View(tracking);
        }

        public async Task<IActionResult> TrackingDelete(string ShipmentCode) // Admin-Tracking-Delete // 
        {
            var exisitingTracking = _db.TrackingDB.FirstOrDefault(t => t.ShipmentCode == ShipmentCode);
            if (exisitingTracking == null)
            {
                return RedirectToAction("Tracking");
            }
            _db.Remove(exisitingTracking);
            await _db.SaveChangesAsync();
            return RedirectToAction("Tracking");
        }
        // ============================================= Tracking ============================================= //

        // ============================================= Payment ============================================= //
        public IActionResult Payment() // Admin-Payment-Index //
        {
            var payment = _db.PaymentDB.Include(t => t.CreditCard).Include(t => t.BankTransfer).Include(t => t.QRData).ToList();
            var cardOptions = _db.CardOptDB.ToList();

            return View(payment);
        }

        // ========== Payment-CreditcardType-Controller ========== //
        public IActionResult PaymentCardOpt(string cardtypequery) // Admin-Payment-Cardtype-Index //
        {
            if (string.IsNullOrEmpty(cardtypequery))
            {
                return View("PaymentCardOpt", _db.CardOptDB.ToList());
            }
            else
            {
                var cardtype = _db.CardOptDB.ToList();
                var filteredcardType = cardtype.Where(searchcardType => searchcardType.CreditcardOpt.Contains(cardtypequery)).ToList();
                return View("PaymentCardOpt", filteredcardType);
            }
        }

        [HttpGet]
        public IActionResult PaymentCardOptCreate() // Admin-Payment-Cardtype-Create(GET) //
        {
            var cardtype = new CreditcardType();
            return View(cardtype);
        }
        [HttpPost]
        [ValidateAntiForgeryToken] // Admin-Payment-Cardtype-Create(POST) //
        public async Task<IActionResult> PaymentCardOptCreate(CreditcardType model)
        {
            if (ModelState.IsValid)
            {
                _db.CardOptDB.Add(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("PaymentCardOpt");
            }
            return View(model);
        }

        public IActionResult PaymentCardOptDelete(int id) // Admin-Payment-Cardtype-Delete //
        {
            var existcreditcardtype = _db.CardOptDB.FirstOrDefault(c => c.Id == id);
            if (existcreditcardtype == null)
            {
                return RedirectToAction("PaymentCardOpt");
            }
            _db.Remove(existcreditcardtype);
            _db.SaveChanges();
            return RedirectToAction("PaymentCardOpt");
        }
        // ========== Payment-CreditcardType-Controller ========== //

        // ========== Payment-Bankaccount-Controller ========== //
        public IActionResult PaymentBankAcc(string bankaccquery) // Admin-Payment-Bankacc-Index //
        {
            if (string.IsNullOrEmpty(bankaccquery))
            {
                return View("PaymentBankAcc", _db.BankAccDB.ToList());
            }
            else
            {
                var bankacc = _db.BankAccDB.ToList();
                var filteredBankacc = bankacc.Where(bankAccount => bankAccount.BankName.Contains(bankaccquery)).ToList();
                return View("PaymentBankAcc", filteredBankacc);
            }
        }

        [HttpGet]
        public IActionResult PaymentBankAccCreate() // Admin-Payment-Bankacc-Create(GET) //
        {
            var bankacc = new BankAccount();
            return View(bankacc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentBankAccCreate(BankAccount model) // Admin-Payment-Bankacc-Create(POST) //
        {
            if (ModelState.IsValid)
            {
                _db.Add(model);
                _db.SaveChanges();
                return RedirectToAction("PaymentBankAcc");
            }
            else
            {
                return RedirectToAction("PaymentBankAccCreate");
            }               
        }

        public IActionResult PaymentBankAccDelete(int id) // Admin-Payment-Bankacc-Delete //
        {
            var existingBankaccount = _db.BankAccDB.FirstOrDefault(ba => ba.Id == id);
            if (existingBankaccount != null) 
            {
                _db.Remove(existingBankaccount);
                _db.SaveChanges();
                return RedirectToAction("PaymentBankAcc");
            }
            else
            {
                return RedirectToAction("PaymentBankAcc");
            }
        }
        // ========== Payment-Bankaccount-Controller ========== //

        // ========== Payment-Promptpay-Controller ========== //
        public IActionResult PaymentPromptpay(string Qrquery) 
        {
            if (string.IsNullOrEmpty(Qrquery)) 
            {
                return View("PaymentPromptpay", _db.StaychillQRDB.ToList());
            }
            else
            {
                var promptpay = _db.StaychillQRDB.ToList();
                var filteredpromptpay = promptpay.Where(qr => qr.Id.ToString().Contains(Qrquery)).ToList();
                return View("PaymentPromptpay", filteredpromptpay);
            }
        }

        [HttpGet]
        public IActionResult PaymentPromptpayEdit(int id)
        {
            var existingQR = _db.StaychillQRDB.FirstOrDefault(qr => qr.Id == id);
            if (existingQR != null)
            {
                return View(existingQR);
            }
            return RedirectToAction("PaymentPromptpay");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentPromptpayEdit(StaychillQR model, IFormFile QR)
        {
            var existingQR = _db.StaychillQRDB.FirstOrDefault(qr => qr.Id == model.Id);
            if (QR != null && QR.Length>0)
            {
                existingQR.StoreQRData = await ConvertToBytes(QR);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("PaymentPromptpay");
        }

        public IActionResult PaymentPromptPayDelete(int id)
        {
            var existingQR = _db.StaychillQRDB.FirstOrDefault(qr => qr.Id == id);
            if (existingQR != null)
            {
                _db.Remove(existingQR);
                _db.SaveChanges();
                return RedirectToAction("PaymentPromptpay");
            }
            else
            {
                return RedirectToAction("PaymentPromptpay");
            }
        }

        // ========== Payment-Promptpay-Controller ========== //

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

        // ============================================= Feedback ============================================= //

        public IActionResult Feedback(string feedbackquery)
        {
            if (string.IsNullOrEmpty(feedbackquery))
            {
                return View("Feedback", _db.FeedbackDB.ToList());
            }
            else
            {
                var feedback = _db.FeedbackDB.ToList();
                var filteredFeedback = feedback.Where(feedback => feedback.Email.Contains(feedbackquery) || feedback.Description.Contains(feedbackquery)).ToList();  
                return View("Feedback",filteredFeedback);
            }
        }

        public IActionResult FeedbackDetail(int id)
        {
            var existingfeedback = _db.FeedbackDB.FirstOrDefault(fb => fb.Id == id);
            if (existingfeedback == null)
            {
                return RedirectToAction("Feedback");
            }
            return View(existingfeedback);
        }

        public IActionResult FeedbackDelete(int id)
        {
            var existingFeedback = _db.FeedbackDB.FirstOrDefault(fb => fb.Id == id);
            if (existingFeedback != null)
            {
                _db.Remove(existingFeedback);
                _db.SaveChanges();
                return RedirectToAction("Feedback");
            }
            return RedirectToAction("Feedback");
        }
    }
}
