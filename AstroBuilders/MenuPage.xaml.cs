using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class MenuPage : ContentPage
	{
		public MenuPage ()
		{
			InitializeComponent ();

			//this.Icon = FileImageSource.FromResource ("AstroBuilders.images.menu_menu.png");
			this.BackgroundColor = Color.FromRgba(220, 220, 255, 255);
			theList.BackgroundColor = this.BackgroundColor;
			theList.ItemsSource = Global.Menus.All;
			theList.ItemSelected += delegate(object sender, SelectedItemChangedEventArgs e) {
				if(e.SelectedItem == null) return;
				Menu m = e.SelectedItem as Menu;
				if(m.Page == MyPage.None) {
					theList.SelectedItem = null;
					return;
				}
				Global.GotoPage (m.Page);
				Global.MainAppPage.IsPresented = false;
			};
		}

	}
}