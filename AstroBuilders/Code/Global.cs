﻿using System;
using Xamarin.Forms;
using AstroBuildersModel;

//using Toasts.Forms.Plugin.Abstractions;
//using Refractored.Xam.Vibrate;
//using Refractored.Xam.Vibrate.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plugin.Toasts;
using Plugin.Vibrate.Abstractions;
using Plugin.Vibrate;

namespace AstroBuilders
{
	public enum MyPage
	{
		None,
		FirstLoading,
		Home,
		Builders,
		Exhibitions,
		Account,
		MyBuilder,
		AdminUsers,
		AdminBuilders,
		MyExhibitions,
		About
	}

	//public delegate void JustTrigger();

	public static class Global
	{
		//public static event JustTrigger FirstLoadingFinish;

		public static readonly Thickness PagePadding = new Thickness (Device.OnPlatform (0, 0, 0), Device.OnPlatform (20, 0, 0), Device.OnPlatform (0, 0, 0), Device.OnPlatform (0, 0, 0));
		public static string BaseUrl = "http://r2builders.diverstrucs.com/";
		//public static string BaseUrl = "http://127.0.0.1:8080/";

		// FIRST...
		/*
		public static Color ColorBackground = Color.FromHex ("132855");
		public static Color ColorText = Color.FromHex ("2F7EA5");
		public static Color ColorHighText = Color.FromHex ("5B70B3");
		public static Color ColorBoxBackground = Color.FromHex ("1F478C");
		public static Color ColorBoxBorder = Color.FromHex ("79BDFA");
		public static Color ColorBoxMiniBorder = Color.FromHex ("3072A4");
		public static Color ColorBoxText = Color.FromHex ("3C81E6");
		public static Color ColorBoxHighText = Color.FromHex ("3BD3E8");
		public static Color ColorBoxLowText = Color.FromHex ("226B98");
		*/

		// SECOND TRY...
		/*
		public static Color ColorBackground = Color.FromHex ("091247");
		public static Color ColorText = Color.FromHex ("2F7EA5");
		public static Color ColorHighText = Color.FromHex ("5AC1D7");
		public static Color ColorBoxBackground = Color.FromHex ("15357A");
		public static Color ColorBoxBorder = Color.FromHex ("72A0E4");
		public static Color ColorBoxMiniBorder = Color.FromHex ("2E5E92");
		public static Color ColorBoxText = Color.FromHex ("3C81E6");
		public static Color ColorBoxHighText = Color.FromHex ("6AD5E5");
		public static Color ColorBoxLowText = Color.FromHex ("66B9C4");

		public static Color Color2Background = Color.FromHex ("4F5C57");
		public static Color Color2BoxBackground = Color.FromHex ("6E6A1A");
		public static Color Color2BoxText = Color.FromHex ("F0DF53");
		public static Color Color2BoxHighText = Color.FromHex ("E7F29C");
		*/

		// Third try ...
		public static Color ColorBackground = Color.FromHex ("77100804");
		public static Color ColorText = Color.FromHex ("B3FBFF");
		public static Color ColorHighText = Color.FromHex ("D7E5E2");
		public static Color ColorBoxBackground = Color.FromHex ("770E181C");
		public static Color ColorBoxBorder = Color.FromHex ("7766A38A");
		public static Color ColorBoxMiniBorder = Color.FromHex ("77162D30");
		public static Color ColorBoxText = Color.FromHex ("236BFE");
		public static Color ColorBoxHighText = Color.FromHex ("A2FFD3");
		public static Color ColorBoxLowText = Color.FromHex ("055C81");

		public static Color Color2Background = Color.FromHex ("040706");
		public static Color Color2BoxBackground = Color.FromHex ("061C12");
		public static Color Color2BoxText = Color.FromHex ("A09535");
		public static Color Color2BoxHighText = Color.FromHex ("F4DE4C");


		public static Dictionary<string, string> Languages = new Dictionary<string, string> ();

		public static MainAppPage MainAppPage;
		public static MenuPage MenuPage;
		public static DetailPage DetailPage;
		public static MenuManager Menus;

		public static CountryManager AllCountry = new CountryManager ();
		public static NewsManager AllNews = new NewsManager ();
		public static BuildersManager AllBuilders = new BuildersManager ();
		public static ClubsManager AllClubs = new ClubsManager ();
		public static UsersManager AllUsers = new UsersManager ();
		public static ExhibitionsManager AllExhibitions = new ExhibitionsManager ();
		public static CardsManager AllCards = new CardsManager ();

		public static Builder CurrentBuilder = null;
		public static Exhibition CurrentExhibition = null;

		public static bool IsConnected = false;
		public static User ConnectedUser = null;

		public static IFiles Files = null;
		public static IToastNotificator Notificator = null;
		public static IVibrate Vibrator = null;
		public static string UniqueAppId = string.Empty;
		public static IBeaconTools BeaconsTools = null;
		public static Plugin.Media.Abstractions.IMedia AllMedia = Plugin.Media.CrossMedia.Current;
		public static IImageResizer ImageResizer = null;

		public static Random Random;
		//public static bool FirstLoading = false;
		//public static bool FirstLoadingInProgress = false;
		//public static bool FirstLoadingError = false;

		public static async void DoInit ()
		{
			if (Files != null)
				return;
			Random = new Random (DateTime.Now.Millisecond);
			Files = DependencyService.Get<IFiles> ();
			Notificator = DependencyService.Get<IToastNotificator> ();
			Vibrator = CrossVibrate.Current;
			Helper.SettingsRead<string> ("UniqueAppId", string.Empty);
			if (UniqueAppId.Length == 0) {
				UniqueAppId = Helper.GenerateAppId;
				Helper.SettingsSave<string> ("UniqueAppId", UniqueAppId);
			}
			BeaconsTools = DependencyService.Get<IBeaconTools> ();
			ImageResizer = DependencyService.Get<IImageResizer> ();
			Tools.DoInit ();
			Menus = new MenuManager ();

			return;
			/*
			Translation.RefreshAllText ();

			Menus.Refresh ();

			if (!Translation.IsTextReady) {
				FirstLoading = true;
				FirstLoadingInProgress = true;
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
						FirstLoadingError = true;
					}
				}
				if (ImmediateResult.Length > 0) {
					//System.Diagnostics.Debug.WriteLine("Traduction: " + ImmediateResult);
					//await Tools.ImmediateDownloadLanguage (Translation.Language);
					Translation.NewTranslation (ImmediateResult);
					Menus.Refresh ();
				} else
					FirstLoadingError = true;
				FirstLoadingInProgress = false;
				if (FirstLoadingFinish != null)
					FirstLoadingFinish ();
			}

			IDataServer allLanguages = new IDataServer ("languages", true);
			allLanguages.DataRefresh += delegate(bool status, string result) {
				System.Diagnostics.Debug.WriteLine ("Status: " + allLanguages.FileName + "=" + status);
				if (!status)
					return;
				System.Diagnostics.Debug.WriteLine ("Result: " + Helper.Decrypt (result));
				SerializableDictionary<string, string> res = null;
				try {
					res =	Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableDictionary<string, string>> (Helper.Decrypt (result));
				} catch (Exception error) {
					System.Diagnostics.Debug.WriteLine ("ERROR: " + error.Message);
				}
				try {
					Translation.AllLanguages.Clear ();
					foreach (KeyValuePair<string, string> kvp in res) {
						Translation.AllLanguages.Add (kvp.Key, kvp.Value);
					}
				} catch (Exception err) { 
					System.Diagnostics.Debug.WriteLine ("** ERROR: " + err.Message);
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
*/
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
		}

		public static void PopulateExhibitions ()
		{
			Global.AllExhibitions.All.Sort ();
			foreach (AstroBuildersModel.Exhibition o in Global.AllExhibitions.All) {
				if (o.BuilderNickname == null || o.BuilderNickname.Length == 0) {
					AstroBuildersModel.Builder b = (AstroBuildersModel.Builder)Global.AllBuilders.GetByGuid<AstroBuildersModel.Builder> (o.IdBuilder);
					o.BuilderNickname = b.NickName;
					AstroBuildersModel.Club c = (AstroBuildersModel.Club)Global.AllClubs.GetByGuid<AstroBuildersModel.Club> (b.IdClub);
					o.ClubName = c.Title;
				}
			}
		}

		public static void PopulateCurrentExhibition ()
		{
			if (CurrentExhibition.AllBuilders == null) {
				foreach (Guid id in CurrentExhibition.Builders) {
					Builder b = (Builder)AllBuilders.GetByGuid<Builder> (id);
					CurrentExhibition.AllBuilders = new ObservableCollection<Builder> ();
					CurrentExhibition.AllBuilders.Add (b);
				}
			}
		}

		public static void StartingBeaconsDetection ()
		{
			BeaconsTools.Founded += BeaconsTools_Founded;
			Dictionary<string, string> info = new Dictionary<string, string> ();
			try {
				info.Add ("R2BUILDERS", "74278BDA-B644-4520-8F0C-720EAF059935"); // HM-10 Default = Apple Air Locate
				info.Add ("estimote", "B9407F30-F5F8-466E-AFF9-25556B57FE6D"); // Estimote Maxxing
			} catch (Exception) {
			}
			BeaconsTools.Init (info);
			// 74278BDA-B644-4520-8F0C-720EAF059935 => HM-10 Default = Apple Air Locate
		}

		static void BeaconsTools_Founded (List<OneBeacon> beacons)
		{
			// on détecte des iBeacons :)

		}

		public static void ShowNotification (ToastNotificationType infoType, string title, string message)
		{
			Device.BeginInvokeOnMainThread (() => {
				Notificator.Notify (infoType, title, message, TimeSpan.FromSeconds (2));
				DoVibrate ();
			});
		}

		public static void DoVibrate ()
		{
			Vibrator.Vibration (500);
		}

		public static void GotoPage (MyPage page)
		{
			AwesomeWrappanel.CleanAll ();
			DetailPage.ShowPage (page);
		}
	}
}