using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Returns view
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}