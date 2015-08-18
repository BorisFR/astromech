using System;
using Xamarin.Forms;
using AstroBuildersModel;
using Toasts.Forms.Plugin.Abstractions;
using Refractored.Xam.Vibrate;
using Refractored.Xam.Vibrate.Abstractions;
using System.Collections.Generic;

namespace AstroBuilders
{
	public enum MyPage
	{
		None,
		Home,
		Builders,
		Account,
		MyBuilder,
		AdminUsers,
		AdminBuilders,
		MyExhibitions,
		About
	}

	public static class Global
	{
		public static readonly Thickness PagePadding = new Thickness(Device.OnPlatform(0, 0, 0), Device.OnPlatform(20, 0, 0), Device.OnPlatform(0, 0, 0), Device.OnPlatform(0, 0, 0));
		public static string BaseUrl = "http://r2builders.diverstrucs.com/";

		public static Dictionary<string, string> Languages = new Dictionary<string, string>();

		public static MainAppPage MainAppPage;
		public static MenuPage MenuPage;
		public static DetailPage DetailPage;
		public static MenuManager Menus;

		public static CountryManager AllCountry = new CountryManager ();
		public static NewsManager AllNews = new NewsManager ();
		public static BuildersManager AllBuilders = new BuildersManager ();
		public static ClubsManager AllClubs = new ClubsManager ();
		public static UsersManager AllUsers = new UsersManager ();
		public static ExhibitionsManager AllExhibitions = new ExhibitionsManager();
		public static CardsManager AllCards = new CardsManager();

		public static Builder CurrentBuilder = null;

		public static bool IsConnected = false;
		public static User ConnectedUser = null;

		public static IFiles Files = null;
		public static IToastNotificator Notificator = null;
		public static IVibrate Vibrator = null;
		public static string UniqueAppId = string.Empty;
		public static IBeaconTools BeaconsTools = null;
		public static Media.Plugin.Abstractions.IMedia AllMedia = Media.Plugin.CrossMedia.Current;

		public static async void DoInit() {
			Files = DependencyService.Get<IFiles> ();
			Notificator = DependencyService.Get<IToastNotificator>();
			Vibrator = CrossVibrate.Current;
			Helper.SettingsRead<string>("UniqueAppId", string.Empty);
			if (UniqueAppId.Length == 0) {
				UniqueAppId = Helper.GenerateAppId;
				Helper.SettingsSave<string>("UniqueAppId", UniqueAppId);
			}
			BeaconsTools = DependencyService.Get<IBeaconTools>();
			Tools.DoInit ();
			Menus = new MenuManager ();
			Menus.Refresh ();

			Translation.RefreshAllText ();

			Menus.Refresh ();

			if (!Translation.IsTextReady) {
				System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient ();
				httpClient.Timeout = new TimeSpan (0, 0, 0, 10, 500);
				httpClient.DefaultRequestHeaders.ExpectContinue = false;
				string url = string.Format("{0}Content/Languages/{1}.txt", Global.BaseUrl, Translation.Language);
				System.Diagnostics.Debug.WriteLine("Url: " + url);
				string ImmediateResult = string.Empty;
				try{
					ImmediateResult = await httpClient.GetStringAsync (url);
				} catch(Exception err) {
					System.Diagnostics.Debug.WriteLine ("Loading language error: " + err.Message);
					try {
						ImmediateResult = await httpClient.GetStringAsync (url);
					} catch (Exception err2) {
						System.Diagnostics.Debug.WriteLine ("Second Loading language error: " + err2.Message);
					}
				}
				if (ImmediateResult.Length > 0) {
					//System.Diagnostics.Debug.WriteLine("Traduction: " + ImmediateResult);
					//await Tools.ImmediateDownloadLanguage (Translation.Language);
					Translation.NewTranslation (ImmediateResult);
					Menus.Refresh ();
				}
			}

			IDataServer allLanguages = new IDataServer ("languages", true);
			allLanguages.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + allLanguages.FileName + "=" + status);
				if(!status)
					return;
				System.Diagnostics.Debug.WriteLine("Result: " + Helper.Decrypt(result));
				SerializableDictionary<string, string> res = null;
				try{
					res =	Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableDictionary<string, string>> (Helper.Decrypt(result));
				}catch(Exception error) {
					System.Diagnostics.Debug.WriteLine("ERROR: " + error.Message);
				}
				Translation.AllLanguages.Clear();
				foreach(KeyValuePair<string, string> kvp in res) {
					Translation.AllLanguages.Add(kvp.Key, kvp.Value);
				}
			};
			DataServer.AddToDo (allLanguages);

			IDataServer xa = new IDataServer ("country", true);
			xa.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + xa.FileName + "=" + status);
				if(!status)
					return;
				AllCountry.LoadFromJson(Helper.Decrypt(result));
			};
			DataServer.AddToDo (xa);

			IDataServer x = new IDataServer ("news", true);
			x.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + x.FileName + "=" + status);
				if(!status)
					return;
				AllNews.LoadFromJson(Helper.Decrypt(result));
				AllNews.Refresh();
			};
			DataServer.AddToDo (x);

			IDataServer xx = new IDataServer ("builders", true);
			xx.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + xx.FileName + "=" + status);
				if(!status)
					return;
				AllBuilders.LoadFromJson(Helper.Decrypt(result));
			};
			DataServer.AddToDo (xx);

			IDataServer xxx = new IDataServer ("clubs", true);
			xxx.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + xxx.FileName + "=" + status);
				if(!status)
					return;
				AllClubs.LoadFromJson(Helper.Decrypt(result));
			};
			DataServer.AddToDo (xxx);

			IDataServer xxxx = new IDataServer ("exhibitions", true);
			xxxx.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + xxxx.FileName + "=" + status);
				if(!status)
					return;
				AllExhibitions.LoadFromJson(Helper.Decrypt(result));
			};
			DataServer.AddToDo (xxxx);


			IDataServer xxxxx = new IDataServer ("cards", true);
			xxxxx.DataRefresh +=  delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine("Status: " + xxxxx.FileName + "=" + status);
				if(!status)
					return;
				AllCards.LoadFromJson(Helper.Decrypt(result));
			};
			DataServer.AddToDo (xxxxx);

			DataServer.Launch ();

			/*
			xa.ForceFreshData = true;
			DataServer.AddToDo (xa);
			x.ForceFreshData = true;
			DataServer.AddToDo (x);
			xx.ForceFreshData = true;
			DataServer.AddToDo (xx);
			xxx.ForceFreshData = true;
			DataServer.AddToDo (xxx);
			*/
			BeaconsTools.Founded += BeaconsTools_Founded;
			BeaconsTools.Init ("R2BUILDERS", "B9407F30-F5F8-466E-AFF9-25556B57FE6D");
			// 74278BDA-B644-4520-8F0C-720EAF059935 => HM-10 Default = Apple Air Locate
		}

		static void BeaconsTools_Founded (List<OneBeacon> beacons)
		{
			// on détecte des iBeacons :)

		}

		public static void ShowNotification (ToastNotificationType infoType, string title, string message) {
			Device.BeginInvokeOnMainThread (() => {
				Notificator.Notify (infoType, title, message, TimeSpan.FromSeconds (2));
				DoVibrate ();
			});
		}

		public static void DoVibrate() {
			Vibrator.Vibration (500);
		}

		public static void GotoPage(MyPage page) {
			AwesomeWrappanel.CleanAll ();
			DetailPage.ShowPage (page);
		}
	}
}