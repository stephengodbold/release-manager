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

        public ReleaseController(IWorkItemService workItemService)
        {
            this.workItemService = workItemService;
        }

        public ActionResult Index(string currentRelease, string previousRelease)
        {
            return View(new ReleaseNotes
                            {
                                Title = "Release Notes",
                                CurrentRelease = "Phase 2_20121123.6",
                                PreviousRelease = "Phase 2_20121123.2",
                                Items = workItemService.GetWorkItems(previousRelease, currentRelease),
                                States = new[] { "Active", "Resolved", "Testing"}
                            });
        }

        public JsonResult Builds(DateTime date)
        {
            return new JsonResult()
                       {
                           Data = new Collection<string>
                                      {
                                          "Continuous_20121126.1",
                                          "Continuous_20121126.2",
                                          "Continuous_20121126.3",
                                      }
                       };
        }

    }
}
