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
        private const string ApiModeKey = "x-api-mode";

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            actionContext.ControllerContext.Request.Properties.Add(ApiModeKey, ExtractApiModeHeader(headers));
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var apiMode = (ApiMode)actionExecutedContext.Request.Properties[ApiModeKey];
            actionExecutedContext.Response.Headers.Add(ApiModeKey, apiMode.ToString());

            base.OnActionExecuted(actionExecutedContext);
        }

        private static ApiMode ExtractApiModeHeader(HttpHeaders headers)
        {
            if (headers == null) throw new ArgumentNullException("headers");
            var apiMode = ApiMode.Demo;

            if (!headers.Contains(ApiModeKey)) return apiMode;

            var headerValue = headers.First(h => h.Key.Equals(ApiModeKey)).Value.First();
            if (!headerValue.Equals("demo", StringComparison.InvariantCultureIgnoreCase))
            {
                apiMode = ApiMode.Production;
            }

            return apiMode;
        }
    }
}