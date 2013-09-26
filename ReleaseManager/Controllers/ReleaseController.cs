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

        public ReleaseController(IWorkItemService workItemService)
        {
            this.workItemService = workItemService;
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
            return null;
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

        public CsvResult ReleaseNotes(string previousRelease, string currentRelease)
        {
            var releaseNotes = workItemService.GetReleaseNotes(previousRelease, currentRelease);

            return new CsvResult
                       {
                           Name = "ReleaseNotes.csv",
                           Content = releaseNotes.CsvItems,
                           ContentEncoding = Encoding.UTF8
                       };
        }
    }
}
