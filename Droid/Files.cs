using System;
using AstroBuilders.Droid;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency (typeof (Files))]

namespace AstroBuilders.Droid
{
	public class Files : IFiles
	{
		private string fileName = string.Empty;

		public Files () { }

		public bool IsExit(string name) {
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = name;
			fileName = Path.Combine (documentsPath, localFilename);
			return File.Exists (fileName);
		}

		public string ReadFile (string name) {
			if (!IsExit (name))
				return string.Empty;
			return File.ReadAllText (fileName);
		}

		public byte[] ReadFileBytes (string name) {
			if (!IsExit (name))
				return null;
			return File.ReadAllBytes (fileName);
		}

		public void SaveFile (string name, string data) {
			DeleteFile (name);
			File.WriteAllText (fileName, data);
		}

		public void SaveFile (string name, byte[] data) {
			DeleteFile (name);
			File.WriteAllBytes (fileName, data);
		}

		public void DeleteFile(string name) {
			if (!IsExit (name))
				return;
			File.Delete (fileName);
		}

	}
}