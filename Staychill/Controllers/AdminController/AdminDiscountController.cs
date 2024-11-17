using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.ViewModel;
using SelectPdf;

namespace Staychill.Controllers.AdminController
{
    public class AdminDiscountController : Controller
    {
        private readonly StaychillDbContext _db;

        public AdminDiscountController(StaychillDbContext db)
        {
            _db = db;
        }


        public IActionResult AdminDiscountIndex()
        {
            var discount = _db.DiscountDB.ToList();
            return View(discount);
        }
    }
}
