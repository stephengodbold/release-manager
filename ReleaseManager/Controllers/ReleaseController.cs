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
        
        

        public ActionResult Index(string currentRelease, string previousRelease)
        {
            try
            {
                
                return View();
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
            

            return Json( 
                new { 
                    
                },
                "application/json",
                Encoding.UTF8
            );
        }

        public CsvResult ReleaseNotes(string previousRelease, string currentRelease)
        {
            

            return new CsvResult
                       {
                           Name = "ReleaseNotes.csv",
                           
                           ContentEncoding = Encoding.UTF8
                       };
        }
    }
}
