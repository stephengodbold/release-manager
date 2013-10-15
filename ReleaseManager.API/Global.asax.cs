using System.Web.Http;
using System.Web.Optimization;
using ReleaseManager.API.App_Start;

namespace ReleaseManager.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalConfiguration.Configuration);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ContainerConfig.RegisterTypes(GlobalConfiguration.Configuration);
        }
    }
}