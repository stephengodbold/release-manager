using System;
using System.Collections.Generic;
using System.Web.Http;
using Autofac.Features.Indexed;
using ReleaseManager.API.App_Start;
using ReleaseManager.API.Queries;
using ReleaseManager.API.Services;
using Environment = ReleaseManager.API.Models.Environment;

namespace ReleaseManager.API.Controllers
{
    public class EnvironmentsController : ApiController, IEnvironmentController
    {
        private readonly IEnvironmentService environmentService;
        private readonly IEnvironmentQuery environmentQuery;

        public EnvironmentsController(
            IIndex<ApiMode, IEnvironmentService> environmentService, 
            IEnvironmentQuery environmentQuery)
        {
            var apiMode = ApiMode.Demo;
            this.environmentService = environmentService[apiMode];
            this.environmentQuery = environmentQuery;
        }

        public IEnumerable<Environment> Get()
        {
            var environments = environmentService.GetEnvironments();
            return environments;
        }

        public Environment Get(string uri)
        {
            var environmentUri = new Uri(uri);
            var environment = environmentQuery.Execute(environmentUri);

            return environment;
        }
    }

    public interface IEnvironmentController
    {
        IEnumerable<Environment> Get();
        Environment Get(string uri);
    }
}
