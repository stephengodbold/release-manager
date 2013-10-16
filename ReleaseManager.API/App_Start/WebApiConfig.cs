using System.Web.Http;
using ReleaseManager.API.Common;

namespace ReleaseManager.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "NotesApi",
                routeTemplate: "api/{controller}/{earlierBuild}/{laterBuild}",
                defaults: new { controller = "Notes", laterBuild = ""}
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "Environments", id = RouteParameter.Optional }
            );
        }
    }
}
