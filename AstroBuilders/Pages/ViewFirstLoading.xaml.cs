using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class ViewFirstLoading : ContentView
	{
		string progressTextBase = "Loading...";

		public ViewFirstLoading ()
		{
			InitializeComponent ();

			progressLabel.Text = progressTextBase;
			Global.FirstLoadingFinish += Global_FirstLoadingFinish;
		}

		void Global_FirstLoadingFinish ()
		{
			Global.FirstLoadingFinish -= Global_FirstLoadingFinish;
			if (Global.FirstLoadingError) {
				Device.BeginInvokeOnMainThread (() => {
					progressLabel.Text = "Impossible de charger le contenu.";
				});
			} else {
				Device.BeginInvokeOnMainThread (() => {
					progressLabel.Text = "Ready!";
				});
				//Global.GotoPage (MyPage.Home);
			}
		}

	}
}