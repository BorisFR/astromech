using System;
using System.Collections.Generic;

namespace AstroBuilders
{
	public static class Translation
	{
		private static Dictionary<string, string> allText = new Dictionary<string, string>();
		private static SerializableDictionary<string, string> allLanguages = new SerializableDictionary<string, string>();
		private static string language = string.Empty;
		private static string fileName = "alltext";

		public static void LoadState(){
			if (language.Length > 0)
				return;
			language = (string)Helper.SettingsRead<string> ("CurrentLanguage", "fr-FR");
		}

		public static string Language {
			get {
				LoadState ();
				return language;
			}
			set {
				language = value;
				Helper.SettingsSave ("CurrentLanguage", language);
			}
		}

		public static SerializableDictionary<string, string> AllLanguages {
			get {
				try {
					allLanguages = (SerializableDictionary<string, string>)Helper.SettingsRead<SerializableDictionary<string, string>> ("AllLanguages", new SerializableDictionary<string, string> ());
				} catch (Exception) {
				}
				if (allLanguages == null)
					allLanguages = new SerializableDictionary<string, string> ();
				return allLanguages;
			}
			set {
				allLanguages = value;
				Helper.SettingsSave ("AllLanguages", allLanguages);
			}
		}

		public static void RefreshAllText(){
			LoadState ();
			if (Global.Files.IsExit (fileName)) {
				System.Diagnostics.Debug.WriteLine ("Read data from file: " + fileName);
				ProvideText (Global.Files.ReadFile (fileName));
			}
		}

		public static string GetString (string name)
		{
			if (allText.ContainsKey (name))
				return allText [name];
			return name;
		}

		public static bool IsTextReady { get { if(allText.Count > 0) return true; return false; } }

		public static void NewTranslation(string data) {
			Global.Files.SaveFile (fileName, data);
			ProvideText (data);
		}

		private static void ProvideText(string data) {
			allText = new Dictionary<string, string> ();
			string[] lines = data.Replace ("\r", "").Split ('\n');
			foreach (string line in lines) {
				string l = line.Trim ();
				if (l.Length == 0)
					continue;
				if (l.StartsWith ("#"))
					continue;
				string[] parts = l.Split ('|');
				if (parts.Length < 2)
					continue;
				try{
				allText.Add (parts [0], parts [1]);
				}catch(Exception err) {
					System.Diagnostics.Debug.WriteLine ("*** ERROR: " + parts [0] + "=" + parts [1] + " / " + err.Message);
				}
			}
		}

	}
}