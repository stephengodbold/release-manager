using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Web.Mvc;
using ReleaseManager.Common;
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
            try
            {
                var model = workItemService.GetReleaseNotes(previousRelease, currentRelease);
                return View(model);
            }
            catch (ArgumentOutOfRangeException)
            {
                var model = new ReleaseNotes
                {
                    Title = "Release Notes",
                    CurrentRelease = currentRelease,
                    PreviousRelease = previousRelease,
                    States = new string[] { },
                    Items = new Collection<WorkItem>()
                };

                return View(model);
            }
        }

        [HttpPost]
        public JsonResult Builds(DateTime date)
        {
            return Json(buildService.GetBuilds(date));
        }

        [HttpPost]
        public JsonResult WorkItems(string previousRelease, string currentRelease)
        {
            var releaseNotes = workItemService.GetReleaseNotes(previousRelease, currentRelease);

            return Json( 
                new { 
                    workitems = releaseNotes.Items,
                    states = releaseNotes.States
                },
                "application/json",
                Encoding.UTF8
            );
        }
    }
}
