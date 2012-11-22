using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ReleaseManager.Common;
using ReleaseManager.Queries;
using ReleaseManager.Services;

namespace ReleaseManager
{
    public class MvcApplication : HttpApplication
    {
        private readonly IWindsorContainer container;

        public MvcApplication()
        {
            container = new WindsorContainer().Install(FromAssembly.This());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            FillContainer();
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
        }

        public void FillContainer()
        {
            container.Register(Classes.FromThisAssembly()
                            .BasedOn<IController>()
                            .LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IEnvironmentService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IConfigurationService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IEnvironmentQuery>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());
        }
        
        public override void Dispose()
        {
            container.Dispose();
            base.Dispose();
        }
    }
}