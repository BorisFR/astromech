﻿using System;
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
			theList.ItemSelected += TheList_ItemSelected;
		}

		void TheList_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			//if (theList.SelectedItem != null)
			//	theList.SelectedItem = null;
		}
	}
}