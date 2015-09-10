using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AstroBuildersModel;

namespace AstroWeb.Controllers
{
    public class BuilderController : Controller
    {
		public ActionResult Index(string id)
        {
			if (!string.IsNullOrEmpty (id)) {
				foreach (Builder builder in Helper.AllBuilders.Collection) {
					if (builder.NickName.Equals (id)) {
						ViewData ["NickName"] = builder.NickName;
						ViewData ["Name"] = builder.Title;
						ViewData ["Location"] = builder.Location;
						ViewData ["Blog"] = builder.Blog;
						ViewData ["Detail"] = builder.Detail;
						ViewData ["Droids"] = builder.Droids;
						ViewData ["Email"] = builder.Email;
						ViewData ["Facebook"] = builder.Facebook;
						ViewData ["Logo"] = builder.Logo;
						ViewData ["Blog"] = builder.Blog;
						Club c = (Club)Helper.AllClubs.GetByGuid<Club> (builder.IdClub);
						ViewData ["ClubLogo"] = c.Logo;
						ViewData ["Club"] = c.Title;
						ViewData ["QRcode"] = string.Format ("http://r2builders.diverstrucs.com/Builder/{0}", builder.NickName);
					}
				}
				return View ();
			}
			return View (Helper.AllBuilders.Collection);
        }
    }
}
