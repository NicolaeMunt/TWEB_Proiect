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

        // Add to HomeController
        public ActionResult TestConnection()
        {
            try
            {
                using (var context = new BusinessLogic.Data.ApplicationDbContext())
                {
                    ViewBag.ConnectionString = context.Database.Connection.ConnectionString;
                    ViewBag.DatabaseExists = context.Database.Exists();
                    ViewBag.CanConnect = false;

                    // Test actual connection
                    context.Database.Connection.Open();
                    ViewBag.CanConnect = true;
                    context.Database.Connection.Close();
                }
                ViewBag.Success = true;
            }
            catch (Exception ex)
            {
                ViewBag.Success = false;
                ViewBag.ErrorMessage = ex.Message;
            }

            return View();
        }

        public ActionResult UserList()
        {
            try
            {
                using (var context = new BusinessLogic.Data.ApplicationDbContext())
                {
                    // Get all users from the database
                    var users = context.Users.ToList();

                    // Get associated login data (optional)
                    var loginData = context.LoginData.ToList();

                    // Pass login data to view for showing last login time
                    ViewBag.LoginData = loginData;

                    // Pass connection status info for debugging
                    ViewBag.ConnectionString = context.Database.Connection.ConnectionString;
                    ViewBag.DatabaseName = context.Database.Connection.Database;

                    return View(users);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.StackTrace = ex.StackTrace;
                return View(new List<Domain.Entities.User.User>());
            }
        }
    }
}