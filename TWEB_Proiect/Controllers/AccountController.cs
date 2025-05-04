using Domain.Entities.User;
using Domain.Interfaces;
using System;
using System.Web;
using System.Web.Mvc;
using TWEB_Project.Models;

namespace TWEB_Project.Controllers
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

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string token;
            var user = _userService.Authenticate(model.Username, model.Password, out token);

            if (user != null)
            {
                // Store token in cookie
                var authCookie = new HttpCookie("AuthToken", token);
                if (model.RememberMe)
                {
                    authCookie.Expires = DateTime.Now.AddDays(1);
                }
                Response.Cookies.Add(authCookie);

                // Store user ID in session for easier access
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;

                return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email
                };

                try
                {
                    bool result = _userService.RegisterUser(user, model.Password);

                    if (result)
                    {
                        return RedirectToAction("Login");
                    }
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "An error occurred during registration.");
                }
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
