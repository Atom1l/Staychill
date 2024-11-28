using Microsoft.AspNetCore.Mvc;
using Staychill.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Staychill.ViewModel;
using Staychill.Models.UserModel;

namespace Staychill.Controllers.UserController
{
    public class UserController : Controller
    {

        public StaychillDbContext _db;
        public UserController(StaychillDbContext db)
        {
            _db = db;
        }

        public IActionResult Login()
        {
            return View();
        }


        // ================= LOG-IN ================= //
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Example: Validate user from the database
                var user = _db.UserDB.SingleOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // Check if the username and password are "Staychill"
                    if (model.Username == "Staychill" && model.Password == "1234")
                    {
                        // Create user claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in the user
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Tracking", "Admin"); // Redirect to another action if username and password are "Staychill"
                    }
                    else
                    {
                        // Create user claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in the user
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("CartIndex", "Cart"); // Redirect to homepage or dashboard
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }


        // ================= LOG-OUT ================= //
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }

        // ================= EDITPROFILE ================= //
        public IActionResult EditProfile(User model)
        {
            var currentUser = User.Identity.Name; // confirm if user exist //
            var existinguser = _db.UserDB.FirstOrDefault(u => u.Username == currentUser);
            if (existinguser == null)
            {
                return NotFound();
            }
            return View(existinguser);
        }
        
    }
}
