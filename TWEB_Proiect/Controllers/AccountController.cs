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
               if (ModelState.IsValid)
               {
                    try
                    {
                         var user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                         if (user != null)
                         {
                              user.LoginTime = DateTime.Now;
                              db.SaveChanges();

                              Session["UserId"] = user.Id;
                              Session["Username"] = user.Username;

                              // Если есть returnUrl, перенаправляем туда, иначе на главную
                              if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                              {
                                   return Redirect(returnUrl);
                              }
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

               ViewBag.ReturnUrl = returnUrl;
               return View(model);
          }

          [AllowAnonymous]
          public ActionResult Register(string returnUrl)
          {
               ViewBag.ReturnUrl = returnUrl;
               return View();
          }

          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public ActionResult Register(RegisterViewModel model, string returnUrl)
          {
               if (ModelState.IsValid)
               {
                    try
                    {
                         // Проверяем уникальность Email
                         var existingUserByEmail = db.Users.FirstOrDefault(u => u.Email == model.Email);
                         if (existingUserByEmail != null)
                         {
                              ModelState.AddModelError("Email", "Un utilizator cu acest email există deja.");
                              ViewBag.ReturnUrl = returnUrl;
                              return View(model);
                         }

                         // Проверяем уникальность Username
                         var existingUserByUsername = db.Users.FirstOrDefault(u => u.Username == model.Username);
                         if (existingUserByUsername != null)
                         {
                              ModelState.AddModelError("Username", "Un utilizator cu acest nume de utilizator există deja.");
                              ViewBag.ReturnUrl = returnUrl;
                              return View(model);
                         }

                         // Создаем нового пользователя
                         var user = new TWEB_Proiect.Domain.Entities.User
                         {
                              Username = model.Username,
                              Email = model.Email,
                              Password = model.Password,
                              FirstName = null, // Используем Username как FirstName
                              LastName = null, // Пустое значение для LastName
                              CreatedDate = DateTime.Now,
                              IsActive = true,
                              LoginTime = DateTime.Now
                         };

                         db.Users.Add(user);
                         db.SaveChanges();

                         // Устанавливаем сессию
                         Session["UserId"] = user.Id;
                         Session["Username"] = user.Username;

                         // Перенаправляем пользователя
                         if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                         {
                              return Redirect(returnUrl);
                         }
                         return RedirectToAction("Index", "Home");
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException dbEx)
                    {
                         // Обработка ошибок уникальности от базы данных
                         var innerMessage = dbEx.InnerException?.InnerException?.Message ?? "";

                         if (innerMessage.Contains("UQ_Users_Email"))
                         {
                              ModelState.AddModelError("Email", "Un utilizator cu acest email există deja.");
                         }
                         else if (innerMessage.Contains("UQ_Users_Username"))
                         {
                              ModelState.AddModelError("Username", "Un utilizator cu acest nume de utilizator există deja.");
                         }
                         else
                         {
                              ModelState.AddModelError("", "Eroare la salvarea în baza de date: " + innerMessage);
                         }
                    }
                    catch (Exception ex)
                    {
                         ModelState.AddModelError("", "Eroare la baza de date: " + ex.Message);
                    }
               }

               ViewBag.ReturnUrl = returnUrl;
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