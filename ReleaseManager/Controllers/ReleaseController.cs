using System;
using System.Collections.ObjectModel;
using System.Web.Mvc;
using ReleaseManager.Models;
using ReleaseManager.Services;

namespace ReleaseManager.Controllers
{
    public class ReleaseController : Controller
    {
        private readonly IWorkItemService workItemService;
        private readonly IBuildService buildService;

        public ReleaseController(IWorkItemService workItemService, IBuildService buildService)
        {
            this.workItemService = workItemService;
            this.buildService = buildService;
        }


        public ActionResult Index(string currentRelease, string previousRelease)
        {
            return View(new ReleaseNotes
                            {
                                Title = "Release Notes",
                                CurrentRelease = "Phase 2_20121123.6",
                                PreviousRelease = "Phase 2_20121123.2",
                                Items = workItemService.GetWorkItems(previousRelease, currentRelease),
                                States = new[] { "Active", "Resolved", "Testing" }
                            });
        }

        [HttpPost]
        public JsonResult Builds(DateTime date)
        {
            return new JsonResult
                        {
                            Data = buildService.GetBuilds(date),
                        };
        }

        [HttpPost]
        public JsonResult WorkItems(string previousRelease, string currentRelease)
        {
            return new JsonResult
            {
                Data = workItemService.GetWorkItems(previousRelease, currentRelease),
            };
        }
    }
}
