using System;
using System.Linq;
using System.Web.Mvc;
using TWEB_Proiect.Data;
using TWEB_Proiect.Domain.Entities;

namespace TWEB_Proiect.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            // Простая статистика пользователей
            try
            {
                ViewBag.UserCount = db.Users.Count();
            }
            catch
            {
                ViewBag.UserCount = 0;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        // Простой метод для отображения пользователей
        public ActionResult Users()
        {
            try
            {
                var users = db.Users.ToList();
                return View(users);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
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