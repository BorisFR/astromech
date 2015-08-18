using System;
using System.Collections.Generic;

namespace AstroBuilders
{
	public static class Translation
	{
		private static string currentLang = string.Empty;
		public static Dictionary<string, string> AllText;
		public static string[] allLanguages;
		private static string language = string.Empty;

		public static void LoadState(){
			if (language.Length > 0)
				return;
			string defaut = "fr-FR";
			language = (string)Helper.SettingsRead<string> ("CurrentLanguage", defaut);
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

		public static string[] AllLanguages {
			get {
				allLanguages = (string[])Helper.SettingsRead<string[]> ("AllLang", null);
				return allLanguages;
			}
			set {
				allLanguages = value;
				Helper.SettingsSave ("AllLang", allLanguages);
			}
		}

		public static string CurrentLang {
			get {
				string defaut = "fr-FR";
				currentLang = (string)Helper.SettingsRead<string> ("CurrentLang", defaut);
				return currentLang;
			}
			set {
				currentLang = value;
				Helper.SettingsSave ("CurrentLang", currentLang);
			}
		}

		public static void RefreshAllText(){
			AllText = (SerializableDictionary<string, string>)Helper.SettingsRead<SerializableDictionary<string, string>> ("AllText", null);
		}

		public static string GetString (string name)
		{
			if (AllText != null) {
				if (AllText.ContainsKey (name))
					return AllText [name];
			}
			return name;
		}

	}
}