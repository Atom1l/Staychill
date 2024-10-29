using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Staychill.Data;
using Staychill.Models;
using Staychill.Models.BankModel;
using Staychill.Models.ProductModel;
using Staychill.ViewModel;

using System.Web;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Mail;


namespace Staychill.Controllers.UserController
{
    public class EmailController : Controller
    {
        private StaychillDbContext _db;
        public EmailController(StaychillDbContext db)
        {
            _db = db;
        }

        public IActionResult SendEmail(string usermail)
        {
            Webmail.SendEmail(usermail);
            return View();
        }
    }
}
