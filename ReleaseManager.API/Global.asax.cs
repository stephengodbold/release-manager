using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ReleaseManager.API.Common;

namespace ReleaseManager.API
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer container;

        public WebApiApplication()
        {
            container = new WindsorContainer().Install(new ContainerConfig());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(container));
        }

        internal class ContainerConfig : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(AllTypes.FromThisAssembly()
                                            .Where(type => type.Name.EndsWith("Controller"))
                                            .WithServiceDefaultInterfaces()
                                            .Configure(controller => controller.LifestylePerWebRequest()));

                container.Register(AllTypes.FromThisAssembly()
                                           .Where(type => type.Name.EndsWith("Query"))
                                           .WithServiceDefaultInterfaces()
                                           .Configure(query => query.LifestylePerWebRequest()));

                container.Register(AllTypes.FromThisAssembly()
                                           .Where(type => type.Name.EndsWith("Service"))
                                           .WithServiceDefaultInterfaces()
                                           .Configure(service => service.LifestylePerWebRequest()));
            }
        }
    }
}