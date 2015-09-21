using System;
using System.Collections.Generic;

using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class PageBuilder : ContentPage
	{
		public PageBuilder ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);
			theContent.Content = new AppearingText (Translation.GetString ("PageBuilderTitle"));

			this.BindingContext = Global.CurrentBuilder;
			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

			Club c = (Club)Global.AllClubs.GetByGuid<Club> (Global.CurrentBuilder.IdClub);
			imgClub.Source = ImageSource.FromUri (new Uri (c.Logo));
			lClub.Text = string.Format ("{0}", c.Title);
			lLocation.Text = string.Format (Translation.GetString ("PageBuilderOrigine"), Global.CurrentBuilder.Location);
			lDroids.Text = string.Format (Translation.GetString ("PageBuilderDroids"), Global.CurrentBuilder.Droids);
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}
	}
}