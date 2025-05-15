using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TWEB_Proiect.Attributes
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Проверяем авторизован ли пользователь через сессию
            return httpContext.Session != null && httpContext.Session["UserId"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Если не авторизован, перенаправляем на страницу входа
            var returnUrl = filterContext.HttpContext.Request.Url != null ?
                           filterContext.HttpContext.Request.Url.ToString() : "";

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Account" },
                    { "action", "Login" },
                    { "returnUrl", returnUrl }
                });
        }
    }
}