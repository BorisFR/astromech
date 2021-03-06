﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AstroBuilders
{
	public enum ProcessStep
	{
		Waiting,
		Loading,
		Processing,
		Ready,
		Broken
	}

	public partial class ViewFirstLoading : ContentView
	{
		private int step = 0;
		private bool isFirst = true;
		private bool isFinish = false;

		public ViewFirstLoading ()
		{
			InitializeComponent ();
			DoStep ();
			//Launch();
			Tools.Trace ("ViewFirstLoading done.");

			theContent.Content = new AppearingText ("Loading data...", Global.ColorHighText, FontSizeResourceExtension.GetLargeValue, FontSizeResourceExtension.GetSmallValue);
		}

		private async void Launch ()
		{
			Device.BeginInvokeOnMainThread (() => {
				DoStep ();
			});
		}

		private List<String> status = new List<string> ();
		private List<int> stepNumber = new List<int> ();
		private const int MAXSTEP = 10;

		private async void ShowStatus (bool uiThread = false)
		{
			try {
				if (uiThread) {
					int sn = stepNumber [0];
					string temp = status [0];
					stepNumber.RemoveAt (0);
					status.RemoveAt (0);
					progressLabel.Text = temp;
					double v = 1.0 * sn / MAXSTEP;
					progressBar.ProgressTo (v, 200, Easing.CubicInOut);
					Tools.Trace ("Step: " + sn.ToString () + " " + temp);
					return;
				}
				Device.BeginInvokeOnMainThread (() => {
					int sn = stepNumber [0];
					string temp = status [0];
					stepNumber.RemoveAt (0);
					status.RemoveAt (0);
					progressLabel.Text = temp;
					double v = sn / MAXSTEP * 1.0;
					progressBar.ProgressTo (v, 200, Easing.CubicInOut);
					Tools.Trace ("Step: " + sn.ToString () + " " + temp);
				});
				await Task.Delay (200);
			} catch (Exception err) {
				Tools.Trace ("ShowStatus: " + err.Message);
			}
		}

		private void SetLabelStage (Label ll, Label ll1, Label ll2, ProcessStep step)
		{
			string text = "^";
			Color c = Color.Accent;
			string progressTextBase = step.ToString ();
			switch (step) {
			case ProcessStep.Waiting:
				text = "^";
				c = Global.ColorBoxLowText;
				break;
			case ProcessStep.Loading:
				text = "&";
				c = Global.ColorBoxText;
				break;
			case ProcessStep.Processing:
				text = "#";
				c = Global.ColorBoxText;
				break;
			case ProcessStep.Ready:
				text = "$";
				c = Global.ColorBoxHighText;
				break;
			case ProcessStep.Broken:
				text = "*";
				c = Global.ColorBoxLowText;
				break;
			}
			if (!isFinish) {
				stepNumber.Add (this.step);
				status.Add (progressTextBase);
			}
			Device.BeginInvokeOnMainThread (() => {
				ShowStatus (true);
				ll.Text = text;
				ll.TextColor = c;
				ll1.TextColor = c;
				ll2.TextColor = c;
			});
		}

		private async Task DoStep ()
		{
			Tools.Trace ("Doing step: " + step);
			if (isFirst) {
				isFirst = false;
				await Task.Delay (500);
			} else
				await Task.Delay (30);

			switch (step) {
			case 0:
				Global.Menus.Refresh ();
				SetLabelStage (l1, l11, l12, ProcessStep.Loading);
				await DoProcessLanguageData ();
				break;
			case 1:
				Global.Menus.Refresh ();
				SetLabelStage (l2, l21, l22, ProcessStep.Loading);
				await DoProcessAllLanguages ();
				break;
			case 2:
				Global.Menus.Refresh ();
				SetLabelStage (l3, l31, l32, ProcessStep.Loading);
				await DoProcessCountries ();
				break;
			case 3:
				SetLabelStage (l4, l41, l42, ProcessStep.Loading);
				await DoProcessClubs ();
				break;
			case 4:
				SetLabelStage (l5, l51, l52, ProcessStep.Loading);
				await DoProcessAllBuilders ();
				break;
			case 5:
				SetLabelStage (l6, l61, l62, ProcessStep.Loading);
				await DoProcessAllDroids ();
				break;
			case 6:
				SetLabelStage (l7, l71, l72, ProcessStep.Loading);
				await DoProcessAllExhibitions ();
				break;
			case 7:
				SetLabelStage (l8, l81, l82, ProcessStep.Loading);
				await DoProcessAllCards ();
				break;
			case 8:
				SetLabelStage (l9, l91, l92, ProcessStep.Loading);
				await DoProcessAllNews ();
				break;
//			case 9:
//				SetLabelStage (l10, l101, l102, ProcessStep.Loading);
//				await DoProcessAllExhibitions ();
//				break;
			case 9:
				while (DataServer.IsJobWaiting) {
					DataServer.Launch ();
					await Task.Delay (1000);
				}
				if (!isFinish) {
					isFinish = true;
					step++;
					await DoStep ();
				}
				break;
			case 10:
				Global.Menus.Refresh ();
				stepNumber.Add (this.step);
				status.Add ("Ready!");
				ShowStatus ();
				Global.StartingBeaconsDetection ();
				isFinish = true;
				Global.MainAppPage.IsPresented = true;
				Tools.Trace ("*******************************");
				break;
			}
		}

		private async Task DoProcessLanguageData ()
		{
			Translation.RefreshAllText ();
			if (Translation.IsTextReady) {
				SetLabelStage (l1, l11, l12, ProcessStep.Ready);
				if (step == 0)
					step++;
				await DoStep ();
				return;
			}
			System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient ();
			httpClient.Timeout = new TimeSpan (0, 0, 0, 10, 500);
			httpClient.DefaultRequestHeaders.ExpectContinue = false;
			string url = string.Format ("{0}Content/Languages/{1}.txt", Global.BaseUrl, Translation.Language);
			//Tools.Trace("Url: " + url);
			string ImmediateResult = string.Empty;
			Tools.Trace ("Loading : " + url);
			try {
				ImmediateResult = await httpClient.GetStringAsync (url);
				//Tools.Trace("Result : " + ImmediateResult);
			} catch (Exception err) {
				Tools.Trace ("Loading language error: " + err.Message);
				//try {
				//    ImmediateResult = await httpClient.GetStringAsync (url);
				//} catch (Exception err2) {
				//    Tools.Trace("Second Loading language error: " + err2.Message);
				SetLabelStage (l1, l11, l12, ProcessStep.Broken);
				//}
			}
			if (ImmediateResult.Length > 0) {
				SetLabelStage (l1, l11, l12, ProcessStep.Processing);
				Translation.NewTranslation (ImmediateResult);
				SetLabelStage (l1, l11, l12, ProcessStep.Ready);
			}
			if (step == 0)
				step++;
			await DoStep ();
		}

		IDataServer allLanguages = new IDataServer ("languages");

		private async Task DoProcessAllLanguages ()
		{
			var z = await allLanguages.HasOldData ();
			if (z) {
				SetLabelStage (l2, l21, l22, ProcessStep.Processing);
				SerializableDictionary<string, string> res = null;
				string x = await allLanguages.OldData ();
				if (x != null && x.Length > 0) {
					res = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableDictionary<string, string>> (Helper.Decrypt (x));
					Translation.AllLanguages.Clear ();
					foreach (KeyValuePair<string, string> kvp in res) {
						Translation.AllLanguages.Add (kvp.Key, kvp.Value);
					}
					SetLabelStage (l2, l21, l22, ProcessStep.Ready);
					if (step == 1)
						step++;
					await DoStep ();
					return;
				}
			}
			allLanguages.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l2, l21, l22, ProcessStep.Processing);
				Tools.Trace ("Status: " + allLanguages.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l2, l21, l22, ProcessStep.Broken);
					if (step == 1) {
						step++;
						DoStep ();
					}
					return;
				}
				Tools.Trace ("Result: " + Helper.Decrypt (result));
				SerializableDictionary<string, string> res = null;
				try {
					res = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializableDictionary<string, string>> (Helper.Decrypt (result));
				} catch (Exception error) {
					Tools.Trace ("ERROR: " + error.Message);
					SetLabelStage (l2, l21, l22, ProcessStep.Broken);
					if (step == 1)
						step++;
					DoStep ();
					return;
				}
				try {
					Translation.AllLanguages.Clear ();
					foreach (KeyValuePair<string, string> kvp in res) {
						Translation.AllLanguages.Add (kvp.Key, kvp.Value);
					}
					SetLabelStage (l2, l21, l22, ProcessStep.Ready);
					if (step == 1)
						step++;
					DoStep ();
					return;
				} catch (Exception err) {
					Tools.Trace ("** ERROR: " + err.Message);
					SetLabelStage (l2, l21, l22, ProcessStep.Broken);
					if (step == 1)
						step++;
					DoStep ();
					return;
				}
			};
			await DataServer.AddToDo (allLanguages);
			DataServer.Launch ();
		}

		IDataServer country = new IDataServer ("country", true);

		private async Task DoProcessCountries ()
		{
			var z = await country.HasOldData ();
			if (z) {
				SetLabelStage (l3, l31, l32, ProcessStep.Processing);
				var x = await country.OldData ();
				Global.AllCountry.LoadFromJson (Helper.Decrypt (x));
				SetLabelStage (l3, l31, l32, ProcessStep.Ready);
				if (step == 2)
					step++;
				DoStep ();
				return;
			}
			country.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l3, l31, l32, ProcessStep.Processing);
				Tools.Trace ("Status: " + country.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l3, l31, l32, ProcessStep.Broken);
					if (step == 2) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					Global.AllCountry.LoadFromJson (Helper.Decrypt (result));
					if (step == 2)
						step++;
					DoStep ();
					SetLabelStage (l3, l31, l32, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l3, l31, l32, ProcessStep.Broken);
					if (step == 2)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (country);
			DataServer.Launch ();
		}

		private async Task DoProcessClubs ()
		{
			IDataServer x = new IDataServer ("clubs", true);
			var a = await x.HasOldData ();
			if (a) {
				SetLabelStage (l4, l41, l42, ProcessStep.Processing);
				var z = await x.OldData ();
				Global.AllClubs.LoadFromJson (Helper.Decrypt (z));
				SetLabelStage (l4, l41, l42, ProcessStep.Ready);
				if (step == 3)
					step++;
				DoStep ();
				return;
			}
			x.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l4, l41, l42, ProcessStep.Processing);
				Tools.Trace ("Status: " + x.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l4, l41, l42, ProcessStep.Broken);
					if (step == 3) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					Global.AllClubs.LoadFromJson (Helper.Decrypt (result));
					if (step == 3)
						step++;
					DoStep ();
					SetLabelStage (l4, l41, l42, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l4, l41, l42, ProcessStep.Broken);
					if (step == 3)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (x);
			DataServer.Launch ();
		}

		private async Task DoProcessAllBuilders ()
		{
			IDataServer x = new IDataServer ("builders", true);
			var a = await x.HasOldData ();
			if (a) {
				SetLabelStage (l5, l51, l52, ProcessStep.Processing);
				var z = await x.OldData ();
				Global.AllBuilders.LoadFromJson (Helper.Decrypt (z));
				SetLabelStage (l5, l51, l52, ProcessStep.Ready);
				if (step == 4)
					step++;
				DoStep ();
				return;
			}
			x.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l5, l51, l52, ProcessStep.Processing);
				Tools.Trace ("Status: " + x.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l5, l51, l52, ProcessStep.Broken);
					if (step == 4) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					Global.AllBuilders.LoadFromJson (Helper.Decrypt (result));
					if (step == 4)
						step++;
					DoStep ();
					SetLabelStage (l5, l51, l52, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l5, l51, l52, ProcessStep.Broken);
					if (step == 4)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (x);
			DataServer.Launch ();
		}

		private async Task DoProcessAllDroids ()
		{
			IDataServer x = new IDataServer ("droids", true);
			var a = await x.HasOldData ();
			if (a) {
				SetLabelStage (l6, l61, l62, ProcessStep.Processing);
				//Global.AllBuilders.LoadFromJson(Helper.Decrypt(x.OldData));
				SetLabelStage (l6, l61, l62, ProcessStep.Ready);
				if (step == 5)
					step++;
				DoStep ();
				return;
			}
			x.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l6, l61, l62, ProcessStep.Processing);
				Tools.Trace ("Status: " + x.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l6, l61, l62, ProcessStep.Broken);
					if (step == 5) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					//Global.AllBuilders.LoadFromJson (Helper.Decrypt (result));
					if (step == 5)
						step++;
					DoStep ();
					SetLabelStage (l6, l61, l62, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l6, l61, l62, ProcessStep.Broken);
					if (step == 5)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (x);
			DataServer.Launch ();
		}

		private async Task DoProcessAllExhibitions ()
		{
			IDataServer x = new IDataServer ("exhibitions", true);
			var a = await x.HasOldData ();
			if (a) {
				SetLabelStage (l7, l71, l72, ProcessStep.Processing);
				var z = await x.OldData ();
				Global.AllExhibitions.LoadFromJson (Helper.Decrypt (z));
				Global.PopulateExhibitions ();
				Global.AllExhibitions.Refresh ();
				SetLabelStage (l7, l71, l72, ProcessStep.Ready);
				if (step == 6)
					step++;
				DoStep ();
				return;
			}
			x.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l7, l71, l72, ProcessStep.Processing);
				Tools.Trace ("Status: " + x.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l7, l71, l72, ProcessStep.Broken);
					if (step == 6) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					Global.AllExhibitions.LoadFromJson (Helper.Decrypt (result));
					Global.PopulateExhibitions ();
					Global.AllExhibitions.Refresh ();
					if (step == 6)
						step++;
					DoStep ();
					SetLabelStage (l7, l71, l72, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l7, l71, l72, ProcessStep.Broken);
					if (step == 6)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (x);
			DataServer.Launch ();
		}

		private async Task DoProcessAllCards ()
		{
			IDataServer x = new IDataServer ("cards", true);
			var a = await x.HasOldData ();
			if (a) {
				SetLabelStage (l8, l81, l82, ProcessStep.Processing);
				var z = await x.OldData ();
				Global.AllCards.LoadFromJson (Helper.Decrypt (z));
				SetLabelStage (l8, l81, l82, ProcessStep.Ready);
				if (step == 7)
					step++;
				DoStep ();
				return;
			}
			x.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l8, l81, l82, ProcessStep.Processing);
				Tools.Trace ("Status: " + x.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l8, l81, l82, ProcessStep.Broken);
					if (step == 7) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					Global.AllCards.LoadFromJson (Helper.Decrypt (result));
					if (step == 7)
						step++;
					DoStep ();
					SetLabelStage (l8, l81, l82, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l8, l81, l82, ProcessStep.Broken);
					if (step == 7)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (x);
			DataServer.Launch ();
		}

		private async Task DoProcessAllNews ()
		{
			IDataServer x = new IDataServer ("news", true);
			var a = await x.HasOldData ();
			if (a) {
				SetLabelStage (l9, l91, l92, ProcessStep.Processing);
				var z = await x.OldData ();
				Global.AllNews.LoadFromJson (Helper.Decrypt (z));
				PopulateNews ();
				Global.AllNews.Refresh ();
				SetLabelStage (l9, l91, l92, ProcessStep.Ready);
				if (step == 8)
					step++;
				DoStep ();
				return;
			}
			x.DataRefresh += delegate(bool status, string result) {
				SetLabelStage (l9, l91, l92, ProcessStep.Processing);
				Tools.Trace ("Status: " + x.FileName + "=" + status);
				if (!status) {
					SetLabelStage (l9, l91, l92, ProcessStep.Broken);
					if (step == 8) {
						step++;
						DoStep ();
					}
					return;
				}
				try {
					Global.AllNews.LoadFromJson (Helper.Decrypt (result));
					PopulateNews ();
					Global.AllNews.Refresh ();
					if (step == 8)
						step++;
					DoStep ();
					SetLabelStage (l9, l91, l92, ProcessStep.Ready);
					return;
				} catch (Exception) {
					SetLabelStage (l9, l91, l92, ProcessStep.Broken);
					if (step == 8)
						step++;
					DoStep ();
					return;
				}
			};
			DataServer.AddToDo (x);
			DataServer.Launch ();
		}

		private void PopulateNews ()
		{
			Global.AllNews.All.Sort ();
			foreach (AstroBuildersModel.News news in Global.AllNews.All) {
				if (news.BuilderNickname == null || news.BuilderNickname.Length == 0) {
					if (news.IdBuilder != Guid.Empty)
						news.BuilderNickname = ((AstroBuildersModel.Builder)Global.AllBuilders.GetByGuid<AstroBuildersModel.Builder> (news.IdBuilder)).NickName;
					else
						news.BuilderNickname = "Boris";
				}
				if (news.ClubName == null || news.ClubName.Length == 0) {
					if (news.IdClub != Guid.Empty)
						news.ClubName = ((AstroBuildersModel.Club)Global.AllClubs.GetByGuid<AstroBuildersModel.Club> (news.IdClub)).Title;
					else
						news.ClubName = "R2 Builders Francophone";
				}
			}
		}


	}
}