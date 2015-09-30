using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class ViewMyExhibitions : ContentView
	{
		public ViewMyExhibitions ()
		{
			InitializeComponent ();
			btCreateExhibition.Clicked += BtCreateExhibition_Clicked;
			theList.ItemsSource = Global.AllExhibitions.AllFromBuilder (Global.ConnectedUser.IdBuilder);
			theList.ItemSelected += TheList_ItemSelected;
			theContent.Content = new AppearingText (Translation.GetString ("ViewMyExhibitionsTitle"));
		}

		void TheList_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			
		}

		void BtCreateExhibition_Clicked (object sender, EventArgs e)
		{
			Navigation.PushModalAsync (new PageCreateExhibition (), true);
		}

	}
}