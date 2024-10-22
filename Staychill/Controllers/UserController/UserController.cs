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

namespace Staychill.Controllers.UserController
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
