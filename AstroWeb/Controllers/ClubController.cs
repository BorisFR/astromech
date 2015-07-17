using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstroWeb.Controllers
{
    public class ClubController : Controller
    {
        public ActionResult Index()
        {
			return View (Helper.AllClubs.Collection);
        }

    }
}