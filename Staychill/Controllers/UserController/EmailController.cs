using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Staychill.Data;
using Staychill.Models;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.ViewModel;
using Staychill.Email;


namespace Staychill.Controllers.UserController
{
    public class EmailController : Controller
    {
        private readonly Email.IEmailSender _emailSender;
        private readonly StaychillDbContext _db;

        public EmailController(Email.IEmailSender emailSender, StaychillDbContext db)
        {
            this._emailSender = emailSender;
            _db = db;
        }


        public async Task<IActionResult> EmailSend() // This action 
        {
            // Will add User soon //

            // variable for email , subject , message //
            var receiver = "atom7548@gmail.com";
            var subject = "Receipt";
            var message = "Here is the list of an item you have done transaction with Staychill Shop";

            await _emailSender.SendEmailAsync(receiver, subject, message); // call SendEmailAsync Task and send the receiver(gmail) , subject , message to send mail //

            return RedirectToAction("Index", "TestUserCreatingAccount"); // Redirect back to Index //
        }
    }
}
