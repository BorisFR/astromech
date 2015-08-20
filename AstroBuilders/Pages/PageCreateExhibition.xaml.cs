using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.IO;
using AstroBuildersModel;

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
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError1"));
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
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError2"));
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
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError3"));
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
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError4"));
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
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError5"));
				return;
			}
			if (entryName.Text == null || entryName.Text.Trim ().Length == 0) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError6"));
				return;
			}
			if (entryDescription.Text == null || entryDescription.Text.Trim ().Length == 0) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError7"));
				return;
			}
			btCreate.IsEnabled = false;
			theAI.IsRunning = true;
			theAI.IsVisible = true;
			Tools.JobDone += Tools_JobDone;
			//Tools.PostMultiPartForm ("http://r2builders.diverstrucs.com/Data/UploadImages", photoLogo, "xxx", "application.jpg", null, string.Empty);
			Exhibition exhibition = new Exhibition ();
			exhibition.Title = entryName.Text.Trim ();
			exhibition.Description = entryDescription.Text.Trim ();
			exhibition.Builders = new List<Guid> ();
			exhibition.EndDate = dateStart.Date;
			exhibition.StartDate = dateStart.Date;
			//exhibition.Flyer = photoFlyer;
			exhibition.IdBuilder = Global.ConnectedUser.IdBuilder;
			exhibition.IdCountry = Global.ConnectedUser.IdCountry;
			//exhibition.Logo = photoLogo;
			Tools.DoCreateExhibition (exhibition);
		}

		void Tools_JobDone (bool status, string result)
		{
			Tools.JobDone -= Tools_JobDone;
			theAI.IsRunning = false;
			theAI.IsVisible = false;
			btCreate.IsEnabled = true;
			try {
				if (status) {
					string json = Helper.Decrypt (result); 
					Global.AllExhibitions.LoadFromJson (json);
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Success, Translation.GetString("NotificationInformation"),Translation.GetString("PageCreateExhibitionMessage1"));
					Navigation.PopModalAsync ();
				} else {
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError8"));
				}
			} catch (Exception err) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString("NotificationError"), Translation.GetString("PageCreateExhibitionError9"));
			}
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}

	}
}