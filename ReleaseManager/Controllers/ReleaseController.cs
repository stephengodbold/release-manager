using System;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Text;
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
            var model = new ReleaseNotes
                        {
                            Title = "Release Notes",
                            CurrentRelease = currentRelease,
                            PreviousRelease = previousRelease,
                            States = new[] { "Active", "Resolved", "Testing" }
                        };

            try
            {
                var items = workItemService.GetWorkItems(previousRelease, currentRelease);
                model.Items = items;
            }
            catch (ArgumentOutOfRangeException)
            {
                model.Items = new Collection<WorkItem>();
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Builds(DateTime date)
        {
            return Json(buildService.GetBuilds(date));
        }

        [HttpPost]
        public JsonResult WorkItems(string previousRelease, string currentRelease)
        {
            return Json( new { workitem = workItemService.GetWorkItems(previousRelease, currentRelease)},
                 "application/json",
                 Encoding.UTF8
            );
        }
    }
}
