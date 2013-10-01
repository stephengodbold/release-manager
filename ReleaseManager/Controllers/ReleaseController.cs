using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web.Mvc;
using ReleaseManager.Common;
using ReleaseManager.Models;
using ReleaseManager.Services;

namespace ReleaseManager.Controllers
{
    public class ReleaseController : Controller
    {
        private const string NotesResource = "notes";

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
                        {"currentRelease", currentRelease},
                        {"previousRelease", previousRelease}
                    };

                model = _restService.GetModel<ReleaseNotes>(NotesResource, parameters);
            }
            catch (WebException)
            {
                model = new ReleaseNotes();
            }

            return View(model);
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
