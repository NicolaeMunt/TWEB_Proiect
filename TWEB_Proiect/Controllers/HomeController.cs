using System;
using System.Linq;
using System.Web.Mvc;
using TWEB_Proiect.Data;

namespace TWEB_Proiect.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ТОЛЬКО ОДИН метод Ask_Question
        public ActionResult Ask_Question()
        {
            return RedirectToAction("Ask", "Question");
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            try
            {
                ViewBag.UserCount = db.Users.Count();
                ViewBag.QuestionCount = db.Questions.Count();
            }
            catch
            {
                ViewBag.UserCount = 0;
                ViewBag.QuestionCount = 0;
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

        public ActionResult Blog()
        {
            ViewBag.Message = "Blog page.";
            return View();
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