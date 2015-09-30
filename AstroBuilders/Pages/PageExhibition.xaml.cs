using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class PageExhibition : ContentPage
	{
		public PageExhibition ()
		{
			InitializeComponent ();
			theContent.Content = new AppearingText (Translation.GetString ("PageExhibitionTitle"));

			Global.PopulateCurrentExhibition ();
			this.BindingContext = Global.CurrentExhibition;

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}

	}
}