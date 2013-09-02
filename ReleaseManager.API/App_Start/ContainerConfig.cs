using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using Autofac;
using Autofac.Integration.WebApi;
using ReleaseManager.API.Common;

namespace ReleaseManager.API.App_Start
{
    public static class ContainerConfig
    {
        public static void RegisterTypes(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(currentAssembly);

            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => t.Name.EndsWith("Query"))
                .AsImplementedInterfaces()
                .InstancePerApiRequest();

            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerApiRequest();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
            config.Services.Replace(typeof(IHttpActionSelector), new DemoActionSelector());
        }
    }
}