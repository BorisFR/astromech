
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AstroWeb
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");

			routes.MapRoute (
				"Builder",
				"Builder/{id}",
				new { controller = "Builder", action = "Index", id = "" }
			);

			routes.MapRoute (
				"TheImage",
				"TheImage/{id}",
				new { controller = "Data", action = "TheImage", id = "" }
			);

			routes.MapRoute (
				"Default",
				"{controller}/{action}/{id}",
				new { controller = "Home", action = "Index", id = "" }
			);

		}

		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}

		protected void Application_Start ()
		{
			AreaRegistration.RegisterAllAreas ();
			RegisterGlobalFilters (GlobalFilters.Filters);
			RegisterRoutes (RouteTable.Routes);
			TheLog.SetBasePath (System.IO.Path.Combine (System.Web.HttpContext.Current.Server.MapPath (@"~/"), "App_Data"));
			TheLog.AddLog (LogType.Debug, "Application_Start");
			Helper.DoInit ();
			Helper.CreateSomeData ();
		}

		protected void Application_End ()
		{
			TheLog.AddLog (LogType.Debug, "Application_End");
			TheLog.Close ();
		}

		protected void Application_Error (Object sender, EventArgs e)
		{
			Exception exception = Server.GetLastError ();
			string requestedUrl = HttpContext.Current.Request.Url.ToString ();

			TheLog.AddLog (LogType.Error, string.Format ("Application_Error: {0} => {1}", requestedUrl, exception.Message));
		}

	}
}
