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
		}

		void BtCreateExhibition_Clicked (object sender, EventArgs e)
		{
			Navigation.PushModalAsync (new PageCreateExhibition (), true);
		}

	}
}