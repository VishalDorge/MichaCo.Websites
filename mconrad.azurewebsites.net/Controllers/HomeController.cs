using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;

namespace mconrad.azurewebsites.net.Controllers
{
    [OutputCache(CacheProfile = "ServerAndClientProfile")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}