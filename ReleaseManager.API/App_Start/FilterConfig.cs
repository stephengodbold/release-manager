using System.Web.Http;
using System.Web.Mvc;
using ReleaseManager.API.Common;

namespace ReleaseManager.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterApiGlobalFilters(HttpConfiguration config)
        {
            config.Filters.Add(new DemoActionAttribute());
        }
    }
}