using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.ProductModel;
using Staychill.Models.ProductModel.DiscountModel;
using Staychill.Models.ProductModel.TrackingModel;
using Staychill.Models.UserModel;
using Staychill.ViewModel;

namespace Staychill.Controllers.UserController
{
    public class FeedbackController : Controller
    {

        private readonly StaychillDbContext _db;
        public FeedbackController(StaychillDbContext db)
        {
            _db = db;
        }

        public IActionResult FeedbackIndex()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FeedbackCreate(string email, string description) // Admin-Discount-Create(POST) //
        {
            
            if (ModelState.IsValid)
            {
                var feedback = new Feedback
                {
                    Email = email,
                    Description = description
                };
                _db.Add(feedback);
                await _db.SaveChangesAsync();
                TempData["Message"] = "Your feedback has been submitted successfully. Thank you!";
                return RedirectToAction("FeedbackIndex");
            }
            TempData["Message"] = "There was an error submitting your feedback. Please try again.";
            return View();
        }
    }
}
