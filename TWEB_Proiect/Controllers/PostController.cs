using System.Web.Mvc;
using TWEB_Proiect.Data;

namespace TWEB_Proiect.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
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