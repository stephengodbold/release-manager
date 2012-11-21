using System.Web.Mvc;
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
            var environments = environmentService.GetEnvironments();
            return View(environments);
        }
    }
}
