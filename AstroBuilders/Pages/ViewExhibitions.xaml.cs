using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class ViewExhibitions : ContentView
	{
		public ViewExhibitions ()
		{
			InitializeComponent ();

			theList.ItemsSource = Global.AllExhibitions.AllExhibitionsGroup;

			theContent.Content = new AppearingText (Translation.GetString ("ViewExhibitionsTitle"));
		}
	}
}