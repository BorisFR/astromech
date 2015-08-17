using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.IO;

namespace AstroBuilders
{
	public partial class PageCreateExhibition : ContentPage
	{

		private bool isPhotoReady = false;
		private byte[] photoData = null;

		public PageCreateExhibition ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add(tapGestureRecognizer);

			btCreate.Clicked += BtCreate_Clicked;
			dateStart.DateSelected += DateStart_DateSelected;
			dateEnd.DateSelected += DateEnd_DateSelected;
			btChooseLogo.Clicked += BtChooseLogo_Clicked;
		}

		void BtChooseLogo_Clicked (object sender, EventArgs e)
		{
			if (Global.AllMedia.IsPickPhotoSupported) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Impossible d'accéder à vos photos.");
				return;
			}
			RealPickPhoto ();
		}

		private async void RealPickPhoto() {
			var file = await Global.AllMedia.PickPhotoAsync ();
			if (file == null)
				return;
			Stream stream = file.GetStream ();
			photoData = new byte[stream.Length];
			stream.Read (photoData, 0, stream.Length);
			isPhotoReady = true;
		}

		void DateEnd_DateSelected (object sender, DateChangedEventArgs e)
		{
			if (dateEnd.Date < dateStart.Date) {
				dateStart.Date = dateEnd.Date;
			}
		}

		void DateStart_DateSelected (object sender, DateChangedEventArgs e)
		{
			if (dateEnd.Date < dateStart.Date) {
				dateEnd.Date = dateStart.Date;
			}
		}

		void BtCreate_Clicked (object sender, EventArgs e)
		{
			if (dateEnd.Date < dateStart.Date) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "La date de fin de la sortie doit être supérieure à la date de début.");
				return;
			}
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}

	}
}