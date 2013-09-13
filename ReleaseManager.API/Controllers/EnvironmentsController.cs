using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Autofac.Features.Indexed;
using ReleaseManager.API.App_Start;
using ReleaseManager.API.Common;
using ReleaseManager.API.Services;
using Environment = ReleaseManager.API.Models.Environment;

namespace ReleaseManager.API.Controllers
{
    public class EnvironmentsController : ApiController, IEnvironmentController
    {
        private readonly IIndex<ApiMode, IEnvironmentService> environmentServices;
        private IEnvironmentService environmentService;

        public EnvironmentsController(
            IIndex<ApiMode, IEnvironmentService> environmentServices)
        {
            this.environmentServices = environmentServices;
        }

        public IEnumerable<Environment> Get()
        {
            ResolveEnvironmentService();

            var environments = environmentService.List();
            return environments;
        }

        public Environment Get(string uri)
        {
            ResolveEnvironmentService();

            try
            {
                var environmentUri = new Uri(uri, UriKind.Absolute);
                var environment = environmentService.Get(environmentUri);
                return environment;
            }
            catch (UriFormatException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
        
        private void ResolveEnvironmentService()
        {
            var apiMode = ControllerContext.Request.GetApiMode();
            environmentService = environmentServices[apiMode];

            
        }
    }

    public interface IEnvironmentController
    {
        IEnumerable<Environment> Get();
        Environment Get(string uri);
    }
}
