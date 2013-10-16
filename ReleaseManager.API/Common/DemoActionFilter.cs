using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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

            if (actionExecutedContext.Response != null)
            {
                actionExecutedContext.Response.Headers.Add("x-api-mode", apiMode.ToString());
            }

            base.OnActionExecuted(actionExecutedContext);
        }

        
    }
}