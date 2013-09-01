using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web.Http.Filters;

namespace ReleaseManager.API.Common
{
    //[AttributeUsage(AttributeTargets.Class)]
    public class DemoActionAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var headerValue = "default";

            if (actionExecutedContext.Request.Headers.Contains("x-api-mode"))
            {
                headerValue = actionExecutedContext.Request.Headers.GetValues("x-api-mode").First();
            }

            actionExecutedContext.Response.Headers.Add("x-api-mode", headerValue);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}