using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mconrad.azurewebsites.net.Controllers
{
    public class DocumentationController : Controller
    {
        public ActionResult Index(string id)
        {
            ViewBag.DocumentId = id;
            return View();
        }
    }
}