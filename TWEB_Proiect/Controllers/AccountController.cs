using System;
using System.Linq;
using System.Web.Mvc;
using TWEB_Proiect.Models;
using TWEB_Proiect.Data;
using TWEB_Proiect.Domain.Entities; // Для User класса

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
                    // Используем TWEB_Proiect.Domain.Entities.User
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
                        ModelState.AddModelError("", "Email sau parolă incorectă.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Eroare la baza de date: " + ex.Message);
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
                        // Используем TWEB_Proiect.Domain.Entities.User
                        var user = new TWEB_Proiect.Domain.Entities.User
                        {
                            Username = model.Username,
                            Email = model.Email,
                            Password = model.Password,
                            LoginTime = DateTime.Now,
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };

                        db.Users.Add(user);
                        db.SaveChanges();

                        Session["UserId"] = user.Id;
                        Session["Username"] = user.Username;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Un utilizator cu acest email există deja.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Eroare la baza de date: " + ex.Message);
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