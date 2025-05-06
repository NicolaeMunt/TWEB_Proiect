using System;
using System.Linq;
using System.Web.Mvc;
using TWEB_Proiect.Models;
using TWEB_Proiect.Data;
using TWEB_Proiect.Domain.Entities;

namespace TWEB_Proiect.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Используем полное имя класса для избежания конфликта
                    var user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                    if (user != null)
                    {
                        user.LoginTime = DateTime.Now;
                        db.SaveChanges();

                        Session["UserId"] = user.Id;
                        Session["Username"] = user.Username;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email or password.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = db.Users.FirstOrDefault(u => u.Email == model.Email);
                    if (existingUser == null)
                    {
                        // Используем полное имя класса
                        var user = new Domain.Entities.User
                        {
                            Username = model.Username,
                            Email = model.Email,
                            Password = model.Password,
                            LoginTime = DateTime.Now
                        };

                        db.Users.Add(user);
                        db.SaveChanges();

                        Session["UserId"] = user.Id;
                        Session["Username"] = user.Username;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User with this email already exists.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}