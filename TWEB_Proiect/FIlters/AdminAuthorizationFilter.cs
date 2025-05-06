using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace TWEB_Proiect.Filters
{
    public class AdminAuthorizationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // Простая проверка сессии
            var userId = filterContext.HttpContext.Session["UserId"];
            if (userId == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }
    }
}