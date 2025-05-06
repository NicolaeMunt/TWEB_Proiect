using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace TWEB_Proiect.Filters
{
    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // Простая проверка аутентификации
            var userId = filterContext.HttpContext.Session["UserId"];
            if (userId == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // Пустая реализация
        }
    }
}