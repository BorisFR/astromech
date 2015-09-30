using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class ViewExhibitions : ContentView
	{
		public ViewExhibitions ()
		{
			InitializeComponent ();

			theList.ItemsSource = Global.AllExhibitions.AllExhibitionsGroup;
			theList.ItemSelected += TheList_ItemSelected;
			theContent.Content = new AppearingText (Translation.GetString ("ViewExhibitionsTitle"));
		}

		void TheList_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (theList.SelectedItem == null)
				return;
			Global.CurrentExhibition = theList.SelectedItem as Exhibition;
			theList.SelectedItem = null;
			Navigation.PushModalAsync (new PageExhibition (), true);
		}

	}
}