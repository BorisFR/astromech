using System;
using System.IO;

namespace AstroWeb
{

	public enum LogType
	{
		Debug,
		Info,
		Error
	}

	public static class TheLog
	{
		private static StreamWriter sw;
		private static int day = -1;
		private static object verrou = new object ();
		private static object verrou2 = new object ();

		private static string basePath = string.Empty;

		public static void SetBasePath (string path)
		{
			basePath = path;
		}

		private static void Init ()
		{
			if (DateTime.UtcNow.Day == day)
				return;
			lock (verrou) {
				if (DateTime.UtcNow.Day == day)
					return;
				lock (verrou2) {
					if (sw != null) {
						sw.Flush ();
						sw.Close ();
						sw = null;
					}
					DateTime dt = DateTime.UtcNow;
					day = dt.Day;
					string path = basePath;
					path = Path.Combine (path, "logs");
					Directory.CreateDirectory (path);
					string file = string.Format ("{0}.txt", DateTime.UtcNow.ToString ("yyyyMMdd"));
					sw = File.AppendText (Path.Combine (path, file));
					sw.AutoFlush = true;
				}
			}
		}

		public static void Close ()
		{
			lock (verrou2) {
				if (sw != null) {
					sw.Flush ();
					sw.Close ();
					sw = null;
				}
				day = -1;
			}
		}

		public static string[] AllLines ()
		{
			lock (verrou) {
				Close ();
				string path = basePath;
				path = Path.Combine (path, "logs");
				string file = string.Format ("{0}.txt", DateTime.UtcNow.ToString ("yyyyMMdd"));
				return File.ReadAllLines (Path.Combine (path, file));
			}
		}

		private static string message;

		public static void AddLog (LogType logType, string text)
		{
			//if (logType == LogType.Debug) return;
			try {
				Init ();
				message = string.Format ("{1};{0};{2}", logType, DateTime.UtcNow.ToString ("yyyyMMdd HHmmss"), text);
				sw.WriteLine (message);
			} catch (Exception e) {
				System.Diagnostics.Debug.WriteLine ("error:" + e.GetBaseException ());
			}
		}

	}
}
