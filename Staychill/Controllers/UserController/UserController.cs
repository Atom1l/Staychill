using Microsoft.AspNetCore.Mvc;
using Staychill.Data;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Staychill.ViewModel;
using Staychill.Models.UserModel;
using Microsoft.EntityFrameworkCore;

namespace Staychill.Controllers.UserController
{
    public class UserController : Controller
    {

        public StaychillDbContext _db;
        public UserController(StaychillDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            if(User.Identity.Name != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return View();
        }


        // ================= LOG-IN ================= //
        public IActionResult Login()
        {
            return View();
        }
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
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Sign in the user
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("ProductMainPage", "Product"); // Redirect to homepage or dashboard
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
        [HttpGet]
        public IActionResult EditProfile(string username)
        {
            // Get the current user's username from identity
            var currentUser = User.Identity.Name;

            // Find the user in the database using the identifier or username
            var existingUser = _db.UserDB.Include(u => u.Address).FirstOrDefault(name => name.Username == username);

            if (existingUser == null)
            {
                return NotFound(); // User not found
            }

            // Map the user details to a UserViewModel to send to the view
            var model = new UserViewModel
            {
                Firstname = existingUser.Firstname,
                Lastname = existingUser.Lastname,
                Email = existingUser.Email,
                Username= existingUser.Username,
                PhoneNumber = existingUser.Phonenumber,
                HouseNumber = existingUser.Address?.Housenumber,
                Alley = existingUser.Address?.Alley,
                Road = existingUser.Address?.Road,
                Subdistrict = existingUser.Address?.Subdistrict,
                District = existingUser.Address?.District,
                Province = existingUser.Address?.Province,
                Country = existingUser.Address?.Country,
                ZipCode = existingUser.Address?.Zipcode
            };

            return View(model); // Pass the model to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _db.UserDB.Include(u=>u.Address).FirstOrDefault(u => u.Username == model.Username);
                if (user == null)
                {
                    return NotFound();
                }
                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = model.Password;
                }

                // Update user details
                user.Firstname = model.Firstname;
                user.Lastname = model.Lastname;
                user.Username = model.Username;
                user.Email = model.Email;
                user.Password = model.Password;
                user.Phonenumber = model.PhoneNumber;
                user.Address.Housenumber = model.HouseNumber;
                user.Address.Alley = model.Alley;
                user.Address.Road = model.Road;
                user.Address.Subdistrict = model.Subdistrict;
                user.Address.District = model.District;
                user.Address.Province = model.Province;
                user.Address.Country = model.Country;
                user.Address.Zipcode = model.ZipCode;

                // Save changes to database
                await _db.SaveChangesAsync();

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login","User"); // Redirect to a profile or success page
            }

            return View(model);

        }



        // ================= SignUp ================= //
        public async Task<IActionResult> SignUp()
        {
            if (User.Identity.Name != null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return View(new UserViewModel());
        }

        // POST:(CREATE) //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel) // userViewModel as a parameter of UserViewModel work as a representing //
        {
            if (ModelState.IsValid) // Check ModelState if it valid or not then proceed to the condition //
            {
                if (_db.UserDB.Any(u => u.Username == userViewModel.Username || u.Email == userViewModel.Email))
                {// Check if there are any Username or Email that already exist in database //
                    ModelState.AddModelError(string.Empty, "Username or Email already exist");
                    return View(userViewModel);
                }

                User newUser = new User() // Creating new object in User.cs with the new variable newUser //
                {
                    // User Info //
                    Firstname = userViewModel.Firstname,
                    Lastname = userViewModel.Lastname,
                    Username = userViewModel.Username,
                    Email = userViewModel.Email,
                    Phonenumber = userViewModel.PhoneNumber,
                    Password = userViewModel.Password,

                    // Address Info //
                    Address = new Address
                    {
                        Housenumber = userViewModel.HouseNumber,
                        Alley = userViewModel.Alley,
                        Road = userViewModel.Road,
                        Subdistrict = userViewModel.Subdistrict,
                        District = userViewModel.District,
                        Province = userViewModel.Province,
                        Country = userViewModel.Country,
                        Zipcode = userViewModel.ZipCode
                    }
                };

                _db.UserDB.Add(newUser); // at UserDB will add newUser data into its database //
                await _db.SaveChangesAsync(); // Save the new data into database //

                return RedirectToAction("ProductMainPage","Product"); // Return to Action name "UserIndex" //
            }

            return View(userViewModel);
        }

    }
}
