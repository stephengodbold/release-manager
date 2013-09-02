using System.Web.Http;
using System.Web.Mvc;
using ReleaseManager.API.Common;

namespace ReleaseManager.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(HttpConfiguration config)
        {
            config.Filters.Add(new DemoActionAttribute());
        }
    }
}