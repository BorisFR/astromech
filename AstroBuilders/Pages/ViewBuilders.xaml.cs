using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class ViewBuilders : ContentView
	{
		public ViewBuilders ()
		{
			InitializeComponent ();
			theList.BackgroundColor = Color.FromRgb (239,239,239);
			theList.Spacing = 0;
			theList.Padding = new Thickness (0);
			var x = new News { Title = "person " + Global.AllNews.Collection.Count };
			Global.AllNews.Add(x);
			theList.ItemsSource = Global.AllBuilders.Collection;
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Button button = sender as Button;
			Guid param = (Guid)button.CommandParameter;
			Builder b = (Builder)Global.AllBuilders.GetByGuid<Builder>(param);
			Global.CurrentBuilder = b;
			System.Diagnostics.Debug.WriteLine ("Clicked: " + param.ToString() + "=" + b.NickName);
			Club c = (Club)Global.AllClubs.GetByGuid<Club>(b.IdClub);
			System.Diagnostics.Debug.WriteLine ("Club: " + c.Title);
			Navigation.PushModalAsync (new PageBuilder (), true);
		}
			
		private void Button_OnClicked(object sender, EventArgs e)
		{
			var x = new News { Title = "person " + Global.AllNews.Collection.Count };
			Global.AllNews.Add(x);
		}

	}
}