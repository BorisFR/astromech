using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstroWeb.Controllers
{
    public class CountryController : Controller
    {
        public ActionResult Index()
        {
			return View (Helper.AllCountry.Collection);
        }

    }
}