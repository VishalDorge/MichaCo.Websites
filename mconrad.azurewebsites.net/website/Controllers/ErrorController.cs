using System;
using System.Linq;
using System.Web.Mvc;

namespace mconrad.azurewebsites.net.Controllers
{
    [OutputCache(CacheProfile = "ServerAndClientProfile")]
    public class ErrorController : Controller
    {
        public ActionResult Index(int? code)
        {
            ViewBag.code = code.HasValue ? code : 0;

            return View("Error");
        }
    }
}