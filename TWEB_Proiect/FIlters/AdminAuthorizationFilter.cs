// TWEB_Project/Filters/AdminAuthorizationFilter.cs
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;
using Domain.Interfaces;

namespace TWEB_Proiect.Filters
{
    public class AdminAuthorizationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var userService = DependencyResolver.Current.GetService<IUserService>();

            // Check if user is authenticated
            var authCookie = filterContext.HttpContext.Request.Cookies["AuthToken"];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            // Check if user is admin
            bool isAdmin = userService.IsUserAdmin(authCookie.Value);
            if (!isAdmin)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" },
                        { "returnUrl", filterContext.HttpContext.Request.RawUrl }
                    });
            }
        }
    }
}