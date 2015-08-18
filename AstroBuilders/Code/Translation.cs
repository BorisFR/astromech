using System;
using System.Collections.Generic;

namespace AstroBuilders
{
	public static class Translation
	{
		private static Dictionary<string, string> allText = new Dictionary<string, string>();
		private static SerializableDictionary<string, string> allLanguages;
		private static string language = string.Empty;

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
				allLanguages = (SerializableDictionary<string, string>)Helper.SettingsRead<SerializableDictionary<string, string>> ("AllLanguages", new SerializableDictionary<string, string>());
				return allLanguages;
			}
			set {
				allLanguages = value;
				Helper.SettingsSave ("AllLanguages", allLanguages);
			}
		}

		public static void RefreshAllText(){
			LoadState ();
			ProvideText ((string)Helper.SettingsRead<string> ("AllText" + language, string.Empty));
		}

		public static string GetString (string name)
		{
			if (allText.ContainsKey (name))
				return allText [name];
			return name;
		}

		public static bool IsTextReady { get { if(allText.Count > 0) return true; return false; } }

		public static void NewTranslation(string data) {
			Helper.SettingsSave ("AllText" + language, data);
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