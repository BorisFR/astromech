using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AstroWeb.Controllers
{
	public class QRcodeController : Controller
	{
		public ActionResult Index ()
		{
			string token = string.Empty;
			try {
				token = Request.Form ["tbMessage"];
			} catch (Exception) {
			}
			return View (token);
		}

	}
}