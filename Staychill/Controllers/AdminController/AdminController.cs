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

                if(normalizedQuery == "0") // To prevent searching 0% and the other like 80%,20% is showing //
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
            var discountId = _db.DiscountDB.FirstOrDefault( d => d.Id == id);
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
            if(existingDiscount == null)
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
                return View("Tracking",filteredTracking);
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
            if(existingTracking == null)
            {
                return RedirectToAction("Tracking");
            }
            existingTracking.Status = editTracking.Status;
            await _db.SaveChangesAsync();
            return RedirectToAction("Tracking");
        }
        public IActionResult TrackingDetail(int paymentmethodId)
        {
            var tracking = _db.TrackingDB.Include(t => t.PaymentMethod)
                .ThenInclude(tc => tc.CreditCard).Include(t => t.PaymentMethod).ThenInclude(tb => tb.BankTransfer)
                .Include(t => t.PaymentMethod).ThenInclude(tq => tq.QRData).FirstOrDefault(t => t.PaymentMethod.PaymentmethodId == paymentmethodId);
            if (tracking == null)
            {
                return RedirectToAction("Payment");
            }
            tracking.RetainCarts = _db.RetaincartsDB
            .Where(rc => rc.Tracking.PaymentMethod.PaymentmethodId == paymentmethodId)  // or however you link retain carts
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
        public IActionResult Payment()
        {
            var tracking = _db.TrackingDB.Include(t => t.PaymentMethod).ToList();
            return View(tracking);
        }


    }
}
