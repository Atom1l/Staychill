﻿using Microsoft.AspNetCore.Mvc;
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
            var currentUser = User.Identity.Name;

            var user = _db.UserDB.FirstOrDefault(u => u.Username == currentUser);

            var bankAccounts = _db.BankAccDB.ToList(); // Make BankAccount into list to be able to see the data inside it //
            var cardOpt = _db.CardOptDB.ToList();


            var cart = _db.CartDB.Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Discount)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product)
                                 .Include(c => c.CartItems).ThenInclude(c => c.Product.Images).Where(c => c.Username == currentUser)
                                 .ToList(); // Make CartDB into list to make a display of the data inside this DB //
            
            var qr = _db.StaychillQRDB.ToList();

            var viewModel = new CartViewModel
            {
                UserEmail = user.Email,
                CartItemDetails = cart, // CartItemDetails as a list<> will contain "cart" data that include all data from CartDb //
                TotalAmount = cart.SelectMany(c => c.CartItems).Select(item => item.TotalPrice - (item.Discount?.DiscountAmount ?? 0)).Sum(),
                TotalDiscountAmount = discountAmount, // Total of the sum of discountamount and total price of product(s) before subtract again //
                DiscountedTotal = discountPrice, // Final total price of the products //
                PaymentViewModel = new PaymentViewModel
                {
                    BankTransfer = new BankTransfer
                    {
                        Accounts = bankAccounts, // To make <Select> be able to track into BankAccDB and use it value to make user choose his/her bank account //
                    },
                    CreditCard = new CreditCard()
                    {
                        CardTypeOpt = cardOpt,
                    }
                },
                StaychillQR = qr,
            };
            return View(viewModel); // return the value with viewModel //
        }
    }
}
