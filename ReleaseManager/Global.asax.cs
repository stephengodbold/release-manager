using System.Linq;
using System.Configuration;
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

            var demoMode = ConfigurationManager.AppSettings.AllKeys.Contains("System.InDemoMode") &&
                           bool.Parse(ConfigurationManager.AppSettings["System.InDemoMode"]);

            RegisterServices(demoMode);
            RegisterQueries();
        }

        private void RegisterServices(bool demoMode)
        {
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IConfigurationService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());

            if (demoMode)
            {
                container.Register(Classes.FromThisAssembly()
                                   .BasedOn<StubEnvironmentService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());
                container.Register(Classes.FromThisAssembly()
                                       .BasedOn<StubBuildService>()
                                       .LifestylePerWebRequest()
                                       .WithServiceAllInterfaces());
                container.Register(Classes.FromThisAssembly()
                                       .BasedOn<StubWorkItemService>()
                                       .LifestylePerWebRequest()
                                       .WithServiceAllInterfaces());
                return;
            }

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<ICsvFormatter>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IEnvironmentService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IWorkItemService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IBuildService>()
                                   .LifestylePerWebRequest()
                                   .WithServiceAllInterfaces());
        }

        private void RegisterQueries()
        {
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IEnvironmentQuery>()
                                   .LifestyleTransient()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IEnvironmentConfigurationQuery>()
                                   .LifestyleTransient()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IServerConfigurationQuery>()
                                   .LifestyleTransient()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IBuildQuery>()
                                   .LifestyleTransient()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IWorkItemCategoryQuery>()
                                   .LifestyleTransient()
                                   .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                    .BasedOn<IBuildWorkItemQuery>()
                                    .LifestyleTransient()
                                    .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IBuildsForDateQuery>()
                                   .LifestyleTransient()
                                   .WithServiceAllInterfaces());
        }

        public override void Dispose()
        {
            container.Dispose();
            base.Dispose();
        }
    }
}