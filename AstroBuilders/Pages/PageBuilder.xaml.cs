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
			this.BindingContext = Global.CurrentBuilder;
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add(tapGestureRecognizer);

			Club c = (Club)Global.AllClubs.GetByGuid<Club>(Global.CurrentBuilder.IdClub);
			imgClub.Source = ImageSource.FromUri (new Uri (c.Logo));
			lClub.Text = string.Format ("{0}", c.Title);
			lLocation.Text = string.Format ("Origine - {0}", Global.CurrentBuilder.Location);
			lDroids.Text = string.Format ("Droïdes - {0}", Global.CurrentBuilder.Droids);
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}
	}
}