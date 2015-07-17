using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class ViewHome : ContentView
	{
		public ViewHome ()
		{
			InitializeComponent ();

			theList.ItemsSource = Global.AllNews.AllNewsGroup;
		}
	}
}