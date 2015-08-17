using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.IO;

namespace AstroBuilders
{
	public partial class PageCreateExhibition : ContentPage
	{

		private bool isPhotoReady = false;
		private byte[] photoLogo = null;
		private byte[] photoFlyer = null;
		private bool choosingLogo = false;

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
			btChooseFlyer.Clicked += BtChooseFlyer_Clicked;
			btTakeLogoPicture.Clicked += BtTakePicture_Clicked;
			btTakeFlyerPicture.Clicked += BtTakeFlyerPicture_Clicked;
		}

		void BtTakeFlyerPicture_Clicked (object sender, EventArgs e)
		{
			choosingLogo = false;
			TakePicture ();
		}

		void BtTakePicture_Clicked (object sender, EventArgs e)
		{
			choosingLogo = true;
			TakePicture ();
		}

		private async void TakePicture() {
			if (!Global.AllMedia.IsTakePhotoSupported) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Impossible d'activer l'appareil photo.");
				return;
			}
			try {
				var file = await Media.Plugin.CrossMedia.Current.TakePhotoAsync (new Media.Plugin.Abstractions.StoreCameraMediaOptions {

					Directory = "AstroBuilders",
					Name = "picture.jpg"
				});

				if (file == null)
					return;
				Stream stream = file.GetStream ();
				if (choosingLogo) {
					photoLogo = new byte[stream.Length];
					stream.Read (photoLogo, 0, (int)stream.Length);
					stream.Position = 0;
					createLogo.Source = ImageSource.FromStream (() => {
						return stream;
					});
				} else {
					photoFlyer = new byte[stream.Length];
					stream.Read (photoFlyer, 0, (int)stream.Length);
					stream.Position = 0;
					createFlyer.Source = ImageSource.FromStream (() => {
						return stream;
					});
				}
				file.Dispose ();
			} catch (Exception) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Une erreur est survenue lors de l'activation de l'appareil photo.");
				return;
			}

		}

		void BtChooseFlyer_Clicked (object sender, EventArgs e)
		{
			choosingLogo = false;
			RealPickPhoto ();
		}

		void BtChooseLogo_Clicked (object sender, EventArgs e)
		{
			choosingLogo = true;
			RealPickPhoto ();
		}

		private async void RealPickPhoto() {
			if (!Global.AllMedia.IsPickPhotoSupported) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Impossible d'accéder à vos images.");
				return;
			}
			try{
			var file = await Global.AllMedia.PickPhotoAsync ();
			if (file == null) {
				if (choosingLogo) {
					createLogo.Source = null;
					photoLogo = null;
				} else {
					createFlyer.Source = null;
					photoFlyer = null;
				}
				return;
			}
			Stream stream = file.GetStream ();
			if (choosingLogo) {
				photoLogo = new byte[stream.Length];
				stream.Read (photoLogo, 0, (int)stream.Length);
				stream.Position = 0;
				createLogo.Source = ImageSource.FromStream (() => {
					return stream;
				});
			} else {
				photoFlyer = new byte[stream.Length];
				stream.Read (photoFlyer, 0, (int)stream.Length);
				stream.Position = 0;
				createFlyer.Source = ImageSource.FromStream (() => {
					return stream;
				});
			}
			file.Dispose ();
			} catch(Exception) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Une erreur est survenue lors du choix de l'image.");
				return;
			}
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