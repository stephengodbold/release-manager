using System.Web.Http;

namespace ReleaseManager.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "NotesApi",
                routeTemplate: "api/{controller}/{earlierBuild}/{laterBuild}"
            );
        }
    }
}
