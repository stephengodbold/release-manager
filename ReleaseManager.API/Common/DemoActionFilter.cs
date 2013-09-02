using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ReleaseManager.API.App_Start;

namespace ReleaseManager.API.Common
{
    public class DemoActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.SetApiMode();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var apiMode = actionExecutedContext.Request.GetApiMode();
            actionExecutedContext.Response.Headers.Add("x-api-mode", apiMode.ToString());

            base.OnActionExecuted(actionExecutedContext);
        }

        
    }
}