using System;
using System.IO;
using System.Web;

namespace AstroWeb
{
	public static class Tools
	{

		public static string LoadTextFile(string name) {
			string file = Path.Combine (HttpContext.Current.Server.MapPath(@"~/App_Data"), name);
			if (!File.Exists (file))
				return string.Empty;
			string res = File.ReadAllText (file);
			return res;
		}

		public static void SaveTextFile(string name, string data) {
			string file = Path.Combine (HttpContext.Current.Server.MapPath(@"~/App_Data"), name);
			File.WriteAllText (file, data);
		}

	}
}