using System.Linq;
using System.Web.Http.Filters;

namespace ReleaseManager.API.Common
{
    //[AttributeUsage(AttributeTargets.Class)]
    public class DemoActionAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var headerValue = "default";
            var headers = actionExecutedContext.Request.Headers;

            if (headers.Contains("x-api-mode"))
            {
                headerValue = headers.First(h => h.Key.Equals("x-api-mode")).Value.First();
            }

            actionExecutedContext.Response.Headers.Add("x-api-mode", headerValue);

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}