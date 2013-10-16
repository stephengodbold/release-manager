using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ReleaseManager.Common;
using ReleaseManager.Models;
using ReleaseManager.Services;

namespace ReleaseManager.Controllers
{
    public class ReleaseController : Controller
    {
        private const string NotesResource = "/notes";

        private readonly IRestService _restService;

        public ReleaseController(IRestService restService)
        {
            _restService = restService;
        }

        public ActionResult Index(string currentRelease, string previousRelease)
        {
            ReleaseNotes model;
            
            try
            {
                var parameters = new Dictionary<string, string>
                    {
                        {"earlierBuild", previousRelease},
                        {"laterBuild", currentRelease}
                    };

                model = _restService.GetModel<ReleaseNotes>(NotesResource, parameters);
                model.States = model.Items.Select(item => item.State).Distinct();
            }
            catch (HttpException)
            {
                model = SetDefaultModel(currentRelease, previousRelease);
            }
            catch (WebException)
            {
                model = SetDefaultModel(currentRelease, previousRelease);
            }

            return View(model);
        }

        private ReleaseNotes SetDefaultModel(string currentRelease, string previousRelease)
        {
            return new ReleaseNotes
                {
                    States = new string[0],
                    CurrentRelease = currentRelease,
                    PreviousRelease = previousRelease,
                    CsvItems = string.Empty,
                    Items = new WorkItem[0],
                    Title = string.Empty
                };
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
