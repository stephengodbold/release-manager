using System;
using System.Collections.Generic;
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
            ResolveBuildService();
            return buildService.GetBuilds(buildDate);
        }

        //get the details for a build
        public ActionResult Get(string identifier)
        {
            ResolveBuildService();
            return new JsonResult();
        }

        private void ResolveBuildService()
        {
            var apiMode = ControllerContext.Request.GetApiMode();
            buildService = buildServiceByMode[apiMode];
        }
    }
}
