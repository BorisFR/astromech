using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();

			//this.Icon = FileImageSource.FromResource ("AstroBuilders.images.menu_menu.png");
			//this.BackgroundColor = Color.FromRgba(220, 220, 255, 255);
			theList.BackgroundColor = this.BackgroundColor;
			theList.ItemsSource = Global.Menus.All;
			theList.ItemSelected += delegate(object sender, SelectedItemChangedEventArgs e) {
				if (e.SelectedItem == null)
					return;
				Menu m = e.SelectedItem as Menu;
				if (m.Page == MyPage.None) {
					theList.SelectedItem = null;
					return;
				}
				Global.GotoPage (m.Page);


//				if (Global.AllBuilders.All.Count == 0) {
//				} else {
//					Builder b = Global.AllBuilders.All [Global.Random.Next (Global.AllBuilders.All.Count)];
//					Global.CurrentBuilder = b;
//					Global.MainAppPage.Detail = new NavigationPage (new PageBuilder ());
//					//Navigation.PushAsync (new PageBuilder ());
//				}



				try { 
					Global.MainAppPage.IsPresented = false;
				} catch (Exception) {
				}
				theList.SelectedItem = null;
			};
			Tools.Trace ("MenuPage done.");
		}

		/*
		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			if (Global.MainAppPage.MasterBehavior == MasterBehavior.Default) {
				System.Diagnostics.Debug.WriteLine ("MenuPage OnAppearing ================================");
				try {
					if (Global.DetailPage.Content.Width < Global.DetailPage.Content.Height) {
						Global.DetailPage.FadeTo (1, 250, Easing.CubicInOut);
						//Global.DetailPage.FadeTo (0.5, 250, Easing.CubicInOut);
					}
				} catch (Exception) {
				}
			} else
				System.Diagnostics.Debug.WriteLine ("MenuPage OnAppearing ================================ " + Global.MainAppPage.MasterBehavior);
		}

		protected override void OnDisappearing ()
		{
			base.OnDisappearing ();

			System.Diagnostics.Debug.WriteLine ("MenuPage OnDisappearing =============================");
			Global.DetailPage.FadeTo (1, 250, Easing.CubicInOut);
		}
		*/

	}
}