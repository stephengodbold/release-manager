using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ReleaseManager.Models;
using ReleaseManager.Services;

namespace ReleaseManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnvironmentService environmentService;

        public HomeController(IEnvironmentService environmentService)
        {
            this.environmentService = environmentService;
        }

        public ActionResult Index()
        {
            IEnumerable<Environment> model;

            try
            {
                model = environmentService.GetEnvironments();
            } 
            catch (WebException)
            {
                model = new Environment[] {};
            }

            return View(model);
        }
    }
}
