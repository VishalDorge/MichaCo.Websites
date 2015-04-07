using System;
using System.Linq;
using System.Web.Mvc;

namespace mconrad.azurewebsites.net.Controllers
{
    [OutputCache(CacheProfile = "ServerAndClientProfile")]
    public class DocumentationController : Controller
    {
        public ActionResult Index(string id = "cachemanager_getting_started")
        {
            ViewBag.Title = id.Replace('_', ' ').Replace("cachemanager", "Cache Manager");
            ViewBag.DocumentId = id;
            return View();
        }
    }
}