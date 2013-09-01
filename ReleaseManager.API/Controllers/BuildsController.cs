using System.Web.Http;
using System.Web.Mvc;

namespace ReleaseManager.API.Controllers
{
    public class BuildsController : ApiController
    {
        //list builds
        public ActionResult Get()
        {
            return new JsonResult();
        }

        //get the details for a build
        public ActionResult Get(string identifier)
        {
            return new JsonResult();
        }
    }
}
