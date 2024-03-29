﻿using System.Net;
using System.Net.Http;
using System.Web.Http;
using Autofac.Features.Indexed;
using ReleaseManager.API.App_Start;
using ReleaseManager.API.Common;
using ReleaseManager.API.Models;
using ReleaseManager.API.Services;

namespace ReleaseManager.API.Controllers
{
    public class NotesController : ApiController
    {

        private readonly IIndex<ApiMode, IWorkItemService> workItemServiceByMode;
        private IWorkItemService workItemService;

        public NotesController(IIndex<ApiMode, IWorkItemService> workItemServiceByMode)
        {
            this.workItemServiceByMode = workItemServiceByMode;
        }

        public ReleaseNotes Get(string laterBuild, string earlierBuild = "")
        {
            
            if (string.IsNullOrWhiteSpace(earlierBuild) || string.IsNullOrWhiteSpace(laterBuild)) 
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest){ReasonPhrase = "Two builds must be specified to obtain release notes"});
            }

            ResolveWorkItemService();
            return workItemService.GetReleaseNotes(earlierBuild, laterBuild);
        }

        private void ResolveWorkItemService()
        {
            var apiMode = ControllerContext.Request.GetApiMode();
            workItemService = workItemServiceByMode[apiMode];
        }

    }
}
