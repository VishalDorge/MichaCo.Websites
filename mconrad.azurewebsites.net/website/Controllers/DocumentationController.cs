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
            ViewBag.Title = "Documentation: " + 
                string.Join(" ",
                    id
                    .Replace("cachemanager", "Cache Manager")
                    .Split('_').Select(p => p.Substring(0, 1).ToUpper() + p.Substring(1)).ToArray());

            ViewBag.DocumentId = id;
            return View();
        }
    }
}