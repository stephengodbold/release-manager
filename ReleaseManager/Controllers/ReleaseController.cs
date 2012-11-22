using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReleaseManager.Controllers
{
    public class ReleaseController : Controller
    {
        public ActionResult Index(string currentRelease, string previousRelease)
        {
            return View();
        }

    }
}
