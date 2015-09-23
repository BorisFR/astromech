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
		private string logoId = string.Empty;
		private string flyerId = string.Empty;

		public PageCreateExhibition ()
		{
			InitializeComponent ();
            
			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += (s, e) => {
				Navigation.PopModalAsync ();
			};
			imgClose.GestureRecognizers.Add (tapGestureRecognizer);

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

		private async void TakePicture ()
		{
			if (!Global.AllMedia.IsTakePhotoSupported) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError1"));
				return;
			}
			try {
				var file = await Media.Plugin.CrossMedia.Current.TakePhotoAsync (new Media.Plugin.Abstractions.StoreCameraMediaOptions {

					Directory = "AstroBuilders",
					Name = "picture.jpg"
				});

				if (file == null) {
					if (choosingLogo) {
						photoLogo = null;
						createLogo.Source = null;
					} else {
						photoFlyer = null;
						createFlyer.Source = null;
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
			} catch (Exception) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError2"));
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

		private async void RealPickPhoto ()
		{
			if (!Global.AllMedia.IsPickPhotoSupported) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError3"));
				return;
			}
			try {
				var file = await Global.AllMedia.PickPhotoAsync ();
				logoId = string.Empty;
				flyerId = string.Empty;
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
			} catch (Exception) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError4"));
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
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError5"));
				return;
			}
			if (entryName.Text == null || entryName.Text.Trim ().Length == 0) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError6"));
				return;
			}
			if (entryDescription.Text == null || entryDescription.Text.Trim ().Length == 0) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError7"));
				return;
			}
			btCreate.IsEnabled = false;
			theAI.IsRunning = true;
			theAI.IsVisible = true;

			//var res = Tools.PostMultiPartForm ("http://r2builders.diverstrucs.com/Data/UploadImages", photoLogo, "file", "application.jpg", null, string.Empty).Result;
			if (!UploadBoth ()) {
				if (!uploadLogo ()) {
					if (!UploadFlyer ()) {
						CreateExhibition ();
					}
				}
			}

		}

		private void CreateExhibition ()
		{
			Exhibition exhibition = new Exhibition ();
			exhibition.Title = entryName.Text.Trim ();
			exhibition.Description = entryDescription.Text.Trim ();
			exhibition.Builders = new List<Guid> ();
			exhibition.EndDate = dateStart.Date;
			exhibition.StartDate = dateStart.Date;
			exhibition.Flyer = flyerId;
			exhibition.IdBuilder = Global.ConnectedUser.IdBuilder;
			exhibition.IdCountry = Global.ConnectedUser.IdCountry;
			exhibition.Logo = logoId;
			uploadingLogo = false;
			uploadingFlyer = false;
			Tools.JobDone += Tools_JobDone;
			Tools.DoCreateExhibition (exhibition);
		}

		private bool UploadBoth ()
		{
			if (photoLogo != null) {
				if (logoId.Length == 0) {
					if (photoFlyer != null) {
						if (flyerId.Length == 0) {
							byte[] logoResize = Global.ImageResizer.ResizeImage (photoLogo, 80, 80);
							byte[] flyerResize = Global.ImageResizer.ResizeImage (photoFlyer, 720, 1080);
							Tools.JobDone += Tools_JobDone;
							uploadingLogo = true;
							uploadingFlyer = true;
							Tools.PostMultiPartForm (string.Format ("{0}Data/UploadImages", Global.BaseUrl), logoResize, "logo.jpg", flyerResize, "flyer.jpg");
							return true;
						}
					}
				}
			}
			return false;
		}

		private bool uploadLogo ()
		{
			if (photoLogo != null) {
				if (logoId.Length == 0) {
					byte[] logoResize = Global.ImageResizer.ResizeImage (photoLogo, 80, 80);
					Tools.JobDone += Tools_JobDone;
					uploadingLogo = true;
					uploadingFlyer = false;
					var res = Tools.PostMultiPartForm (string.Format ("{0}Data/UploadImages", Global.BaseUrl), logoResize, "logo.jpg").Result;
					return true;
				}
			}
			return false;
		}

		private bool UploadFlyer ()
		{
			if (photoFlyer != null) {
				if (flyerId.Length == 0) {
					byte[] flyerResize = Global.ImageResizer.ResizeImage (photoFlyer, 720, 1080);
					Tools.JobDone += Tools_JobDone;
					uploadingLogo = false;
					uploadingFlyer = true;
					var res = Tools.PostMultiPartForm (string.Format ("{0}Data/UploadImages", Global.BaseUrl), flyerResize, "flyer.jpg").Result;
					return true;
				}
			}
			return false;
		}

		private bool uploadingLogo = false;
		private bool uploadingFlyer = false;

		void Tools_JobDone (bool status, string result)
		{
			Tools.JobDone -= Tools_JobDone;
			try {
				if (status) {
					string json = Helper.Decrypt (result); 
					if (uploadingLogo && uploadingFlyer) {
						List<KeyValuePair<string, string>> res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>> (json);
						logoId = res [0].Value;
						flyerId = res [1].Value;
						CreateExhibition ();
					} else if (uploadingLogo) {
						List<KeyValuePair<string, string>> res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>> (json);
						logoId = res [0].Value;
						if (!UploadFlyer ()) {
							CreateExhibition ();
						}
						return;
					} else if (uploadingFlyer) {
						List<KeyValuePair<string, string>> res = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>> (json);
						flyerId = res [0].Value;
						CreateExhibition ();
						return;
					} else {
						Global.AllExhibitions.LoadFromJson (json);
					}
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Success, Translation.GetString ("NotificationInformation"), Translation.GetString ("PageCreateExhibitionMessage1"));
					Navigation.PopModalAsync ();
				} else {
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError8"));
				}
			} catch (Exception err) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("PageCreateExhibitionError9"));
			}
			theAI.IsRunning = false;
			theAI.IsVisible = false;
			btCreate.IsEnabled = true;
		}

		void ButtonClicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}

	}
}