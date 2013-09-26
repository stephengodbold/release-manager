using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ReleaseManager.Models;
using ReleaseManager.Services;

namespace ReleaseManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigurationService configurationService;

        public HomeController(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public ActionResult Index()
        {
            IEnumerable<Environment> model;

            var rootUri = configurationService.GetRootUri();

            try
            {
            } 
            catch (WebException)
            {
                model = new Environment[] {};
            }

            return View(model);
        }
    }
}
