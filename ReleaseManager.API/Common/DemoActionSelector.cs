using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;

namespace ReleaseManager.API.Common
{
    public class DemoActionSelector : ApiControllerActionSelector
    {
        public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
        {
            var typeInfo = controllerContext.Controller.GetType();
            var targetActionName = controllerContext.RouteData.Values["action"].ToString();
            
            var apiMode = controllerContext.Request.Headers.GetValues("x-api-mode").FirstOrDefault();
            apiMode = apiMode ?? "default";

            if (apiMode.Equals("demo", StringComparison.InvariantCultureIgnoreCase))
            {
                targetActionName = targetActionName + "demo";
            }

            var targetAction = typeInfo.GetMethod(targetActionName);

            if (targetAction == null)
            {
                throw new HttpException(404, string.Format("The action {0} could not be found", targetActionName));
            }

            return new ReflectedHttpActionDescriptor(controllerContext.ControllerDescriptor,
                                                                     targetAction);
        }
    }
}