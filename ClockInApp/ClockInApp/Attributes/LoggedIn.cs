using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;



namespace ClockInApp.Attributes
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ctx = context.HttpContext;

            if ((ctx.Session.Keys.Count() == 0) || (ctx.Session.GetString("UserName").Trim().Length == 0)){
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Login" }));
            }

            base.OnActionExecuting(context);
        }
    }
}
