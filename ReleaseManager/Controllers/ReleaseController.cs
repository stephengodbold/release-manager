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

    }
}
