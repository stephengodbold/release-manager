using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using ReleaseManager.Services;

namespace ReleaseManager.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterTypes()
        {
            var builder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterControllers(currentAssembly);

            builder.RegisterType<ConfigurationService>()
                   .As<IConfigurationService>()
                   .InstancePerHttpRequest();


            builder.RegisterModule(new AutofacWebTypesModule());

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}