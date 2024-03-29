﻿using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using ReleaseManager.API.Common;
using ReleaseManager.API.Services;

namespace ReleaseManager.API.App_Start
{
    public enum ApiMode
    {
        Demo,
        Production
    }

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

            RegisterServices(builder);

            config.DependencyResolver = new AutofacWebApiDependencyResolver(builder.Build());
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<WorkItemService>()
                .Keyed<IWorkItemService>(ApiMode.Production)
                .As<IWorkItemService>()
                .InstancePerApiRequest();

            builder.RegisterType<WorkItemServiceStub>()
                .Keyed<IWorkItemService>(ApiMode.Demo)
                .As<IWorkItemService>()
                .InstancePerApiRequest();

            builder.RegisterType<BuildService>()
                .Keyed<IBuildService>(ApiMode.Production)
                .As<IBuildService>()
                .InstancePerApiRequest();

            builder.RegisterType<BuildServiceStub>()
                .Keyed<IBuildService>(ApiMode.Demo)
                .As<IBuildService>()
                .InstancePerApiRequest();

            builder.RegisterType<EnvironmentService>()
                .Keyed<IEnvironmentService>(ApiMode.Production)
                .As<IEnvironmentService>()
                .InstancePerApiRequest();

            builder.RegisterType<EnvironmentServiceStub>()
                .Keyed<IEnvironmentService>(ApiMode.Demo)
                .As<IEnvironmentService>()
                .InstancePerApiRequest();

            builder.RegisterType<WorkItemCsvFormatter>()
                .As<IWorkItemFormatter<string>>()
                .InstancePerApiRequest();

            builder.RegisterType<ConfigurationService>()
                .As<IConfigurationService>()
                .InstancePerApiRequest();

        }
    }
}