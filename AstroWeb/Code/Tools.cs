using System;
using System.IO;
using System.Web;

namespace AstroWeb
{
	public static class Tools
	{

		public static void CreateFolder(string name) {
			string folder = Path.Combine (HttpContext.Current.Server.MapPath(@"~/App_Data"), name);
			System.Diagnostics.Debug.WriteLine ("Create folder " + folder);
			Directory.CreateDirectory (folder);
		}

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


		public static void SaveTextFile(string folder, string name, string data) {
			string path = Path.Combine (HttpContext.Current.Server.MapPath (@"~/App_Data"), folder);
			string file = Path.Combine (path, name + ".json");
			File.WriteAllText (file, data);
		}

		public static void DeleteTextFile(string folder, string name) {
			string path = Path.Combine (HttpContext.Current.Server.MapPath (@"~/App_Data"), folder);
			string file = Path.Combine (path, name + ".json");
			if (!File.Exists (file))
				return;
			File.Delete (file);
		}

	
		public static bool IsDirectoryEmpty(string folder) {
			string path = Path.Combine (HttpContext.Current.Server.MapPath (@"~/App_Data"), folder);
			string[] files = Directory.GetFiles (path, "*.json", SearchOption.TopDirectoryOnly);
			if (files == null)
				return true;
			if (files.Length == 0)
				return true;
			return false;
		}

		public static string[] GetFiles(string folder) {
			string path = Path.Combine (HttpContext.Current.Server.MapPath (@"~/App_Data"), folder);
			string[] files = Directory.GetFiles (path, "*.json", SearchOption.TopDirectoryOnly);
			return files;
		}

	}
}