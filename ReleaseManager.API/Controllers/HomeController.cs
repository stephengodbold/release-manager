using System.Web.Mvc;

namespace ReleaseManager.API.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
