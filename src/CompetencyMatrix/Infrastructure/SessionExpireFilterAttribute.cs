using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace CompetencyMatrix.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = filterContext.HttpContext;

            // check if session is supported
            byte[] result;
            if (ctx.Session.TryGetValue("CurrentUserId", out result))
            {

            }
            else
            {
                // For round-trip posts, we're forcing a redirect to Home/TimeoutRedirect/, which
                // simply displays a temporary 5 second notification that they have timed out, and
                // will, in turn, redirect to the logon page.
                var isAjax = ctx.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
                if (isAjax)
                {
                    throw new TimeExpiredException("TimeExpired");
                }
                else
                {
                    string redirectTo = "~/Account/ReLogin";
                    filterContext.Result = new RedirectResult(redirectTo);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
