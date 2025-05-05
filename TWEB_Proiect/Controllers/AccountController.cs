using Domain.Entities.User;
using Domain.Interfaces;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TWEB_Proiect.Models;

namespace TWEB_Proiect.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                // Log attempt for debugging
                System.Diagnostics.Debug.WriteLine($"Login attempt: {model.Username}");

                if (!ModelState.IsValid)
                {
                    // Return to login with validation errors
                    return View(model);
                }

                string token;
                var user = _userService.Authenticate(model.Username, model.Password, out token);

                if (user != null)
                {
                    // Login successful
                    System.Diagnostics.Debug.WriteLine($"Login successful for: {model.Username}");

                    // Store token in cookie
                    var authCookie = new HttpCookie("AuthToken", token);
                    if (model.RememberMe)
                    {
                        authCookie.Expires = DateTime.Now.AddDays(1);
                    }
                    Response.Cookies.Add(authCookie);

                    // Store user info in session
                    Session["UserId"] = user.Id;
                    Session["Username"] = user.Username;

                    // Success message
                    TempData["SuccessMessage"] = "Login successful!";

                    // Always redirect to Index after successful login
                    return RedirectToAction("Index", "Home");
                }

                // Authentication failed
                System.Diagnostics.Debug.WriteLine($"Authentication failed for: {model.Username}");
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }
            catch (Exception ex)
            {
                // Log exception
                System.Diagnostics.Debug.WriteLine($"Login exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // Add error to model state
                ModelState.AddModelError("", $"Error during login: {ex.Message}");
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // In AccountController.cs - Updated Register action
        
        public ActionResult Register(RegisterViewModel model)
        {
            try
            {
                // Log attempt
                System.Diagnostics.Debug.WriteLine($"Registration attempt: {model.Username}, {model.Email}");

                if (ModelState.IsValid)
                {
                    var user = new User
                    {
                        Username = model.Username,
                        Email = model.Email
                    };

                    System.Diagnostics.Debug.WriteLine("Calling RegisterUser service method");
                    bool result = _userService.RegisterUser(user, model.Password);

                    System.Diagnostics.Debug.WriteLine($"RegisterUser result: {result}, User ID: {user.Id}");

                    if (result)
                    {
                        // Registration successful
                        TempData["SuccessMessage"] = "Registration successful!";

                        // Redirect to UserList instead of Login
                        return RedirectToAction("UserList", "Home");
                    }
                    else
                    {
                        // Registration returned false but no exception
                        System.Diagnostics.Debug.WriteLine("Registration failed without exception");
                        ModelState.AddModelError("", "Registration failed. Please try again.");
                    }
                }
                else
                {
                    // Log validation errors
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage);
                    System.Diagnostics.Debug.WriteLine($"Validation errors: {string.Join(", ", errors)}");
                }
            }
            catch (ApplicationException ex)
            {
                // Expected application exceptions (like duplicate username)
                System.Diagnostics.Debug.WriteLine($"Registration application exception: {ex.Message}");
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception ex)
            {
                // Unexpected exceptions
                System.Diagnostics.Debug.WriteLine($"Registration exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Error during registration: {ex.Message}");
            }

            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // Clear authentication cookie and session
            if (Request.Cookies["AuthToken"] != null)
            {
                var c = new HttpCookie("AuthToken")
                {
                    Expires = DateTime.Now.AddDays(-1)
                };
                Response.Cookies.Add(c);
            }

            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        // Helper method
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
