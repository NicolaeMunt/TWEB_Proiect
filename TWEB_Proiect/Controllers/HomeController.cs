using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace TWEB_Proiect.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        public ActionResult Ask_Question()
        {
            return View();
        }

        public ActionResult LogIn()
        {
            ViewBag.HideFooter = true;   // layout reads this flag
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            ViewBag.HideFooter = true;
            return View();
        }

        public ActionResult TestDbConnection()
        {
            try
            {
                using (var context = new BusinessLogic.Data.ApplicationDbContext())
                {
                    // Try to access the database
                    bool canConnect = context.Database.Exists();
                    ViewBag.ConnectionStatus = canConnect ? "Connection successful!" : "Database doesn't exist";
                    ViewBag.DbName = context.Database.Connection.Database;
                    ViewBag.ConnectionString = context.Database.Connection.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ConnectionStatus = "Connection failed!";
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }
    }
}