using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using Autofac.Features.Indexed;
using ReleaseManager.API.App_Start;
using ReleaseManager.API.Common;
using ReleaseManager.API.Models;
using ReleaseManager.API.Services;

namespace ReleaseManager.API.Controllers
{
    public class BuildsController : ApiController
    {
        private readonly IIndex<ApiMode, IBuildService> buildServiceByMode;
        private IBuildService buildService;

        public BuildsController(IIndex<ApiMode, IBuildService> buildServiceByMode)
        {
            this.buildServiceByMode = buildServiceByMode;
        }

        public IEnumerable<Build> Get()
        {
            ResolveBuildService();
            return buildService.GetBuilds(DateTime.Today);
        }

        //list builds
        public IEnumerable<Build> Get(DateTime buildDate)
        {
            if (buildDate > DateTime.Today)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            ResolveBuildService();
            return buildService.GetBuilds(buildDate);
        }

        //get the details for a build
        public Build Get(string identifier)
        {
            ResolveBuildService();

            var build = buildService.GetBuild(identifier);

            if (build == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return build;
        }

        private void ResolveBuildService()
        {
            var apiMode = ControllerContext.Request.GetApiMode();
            buildService = buildServiceByMode[apiMode];
        }
    }
}
