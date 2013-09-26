using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Web.Mvc;
using ReleaseManager.Models;
using ReleaseManager.Services;
using RestSharp;

namespace ReleaseManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfigurationService _configurationService;

        public HomeController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public ActionResult Index()
        {
            IEnumerable<Environment> model;

            var rootUri = _configurationService.GetRootUri();
            var demoMode = _configurationService.GetMode();

            try
            {
                var client = new RestClient(rootUri);
                var request = new RestRequest("/environments", Method.GET);
                request.AddHeader("x-api-mode", demoMode);

                var response = client.Execute<Collection<Environment>>(request);

                model = response.Data;
            } 
            catch (WebException)
            {
                model = new Environment[] {};
            }

            return View(model);
        }
    }
}
