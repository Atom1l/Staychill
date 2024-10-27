using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.ViewModel;

namespace Staychill.Controllers.UserController
{
    public class PaymentController : Controller
    {
        private readonly StaychillDbContext _db;
        public PaymentController(StaychillDbContext db)
        {
            _db = db;
        }

        // ========== DISPLAY ========== //
        public IActionResult PaymentIndex(float discountAmount, float discountPrice)
        {
            var bankAccounts = _db.BankAccDB.ToList();

            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product.Images).ToList();

            var viewModel = new CartViewModel
            {
                CartItemDetails = cart, // CartItemDetails as a list<> will contain "cart" data that include all data from CartDb //
                TotalAmount = cart.SelectMany(c => c.CartItems).Select(item => item.TotalPrice - (item.Discount?.DiscountAmount ?? 0)).Sum(),
                TotalDiscountAmount = discountAmount,
                DiscountedTotal = discountPrice,
                PaymentViewModel = new PaymentViewModel
                {
                    BankTransfer = new BankTransfer
                    {
                        Accounts = bankAccounts,
                    }
                }
                
            };

            return View(viewModel);
        }
    }
}
