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
        private readonly IIndex<ApiMode, IEnvironmentService> environmentServiceByMode;
        private IEnvironmentService environmentService;

        public EnvironmentsController(
            IIndex<ApiMode, IEnvironmentService> environmentServiceByMode)
        {
            this.environmentServiceByMode = environmentServiceByMode;
        }

        public IEnumerable<Environment> Get()
        {
            ResolveEnvironmentService();

            return environmentService.List();
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
            environmentService = environmentServiceByMode[apiMode];
        }
    }

    public interface IEnvironmentController
    {
        IEnumerable<Environment> Get();
        Environment Get(string uri);
    }
}
