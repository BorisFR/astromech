using System;
using AstroBuilders.Droid;
using Xamarin.Forms;
using System.IO;
using System.Threading.Tasks;

[assembly: Dependency (typeof (Files))]

namespace AstroBuilders.Droid
{
	public class Files : IFiles
	{
		private string fileName = string.Empty;

		public Files () { }

		private bool isExit(string name) {
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = name;
			fileName = Path.Combine (documentsPath, localFilename);
			return File.Exists (fileName);
		}

		public async Task<bool> IsExit(string name) {
			return isExit (name);
		}

		public async Task<string> ReadFile (string name) {
			if (!isExit (name))
				return string.Empty;
			return File.ReadAllText (fileName);
		}

		public async Task<byte[]> ReadFileBytes (string name) {
			if (!isExit (name))
				return null;
			return File.ReadAllBytes (fileName);
		}

		public async Task SaveFile (string name, string data) {
			DeleteFile (name);
			File.WriteAllText (fileName, data);
		}

		public async Task SaveFile (string name, byte[] data) {
			DeleteFile (name);
			File.WriteAllBytes (fileName, data);
		}

		public async Task DeleteFile(string name) {
			if (!isExit (name))
				return;
			File.Delete (fileName);
		}

	}
}