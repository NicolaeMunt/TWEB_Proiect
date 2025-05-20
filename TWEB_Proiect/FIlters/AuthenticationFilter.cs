using Domain.Interfaces;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace TWEB_Proiect.Filters
{

    public class AuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        // private readonly IUserService _userService;

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //  _userService = userService;
            var userService = DependencyResolver.Current.GetService<IUserService>();

            var authCookie = filterContext.HttpContext.Request.Cookies["AuthToken"];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            int? userId = filterContext.HttpContext.Session["UserId"] as int?;
            if (!userId.HasValue)
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
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


       /* public void OnAuthentication(AuthenticationContext filterContext)
        {
            // Check if user is authenticated
            var authCookie = filterContext.HttpContext.Request.Cookies["AuthToken"];
            if (authCookie == null || string.IsNullOrEmpty(authCookie.Value))
            {
                filterContext.Result = new HttpUnauthorizedResult();
                return;
            }

            // Validate token
            bool isValid = _userService.ValidateToken(authCookie.Value);
            if (!isValid)
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
       */