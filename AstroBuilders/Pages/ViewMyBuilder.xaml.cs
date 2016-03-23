using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class ViewMyBuilder : ContentView
	{
		private Builder builder = null;

		public ViewMyBuilder ()
		{
			InitializeComponent ();

			builder = ((Builder)Global.AllBuilders.GetByGuid<Builder> (Global.ConnectedUser.IdBuilder)).DeepCopy ();

			Club club = (Club)Global.AllClubs.GetByGuid<Club> (builder.IdClub);
			textClub.Text = string.Format (Translation.GetString ("ViewMyBuilderMyClub"), club.Title);
			logoClub.Source = club.Logo;

			entryLogo.TextChanged += EntryLogo_TextChanged;
			theStack.BindingContext = builder;

			btValidate.Clicked += BtValidate_Clicked;
		}

		void BtValidate_Clicked (object sender, EventArgs e)
		{
			btValidate.IsEnabled = false;
			theAI.IsRunning = true;
			theAI.IsVisible = true;
			Tools.JobDone += Tools_UpdateDone;
			Tools.DoUpdateBuilder (builder);
		}

		void Tools_UpdateDone (bool status, string result)
		{
			Tools.JobDone -= Tools_UpdateDone;
			try {
				if (status) {
					string json = Helper.Decrypt (result); //Tools.Result);
					Global.AllBuilders.LoadFromJson (json);
					builder = ((Builder)Global.AllBuilders.GetByGuid<Builder> (Global.ConnectedUser.IdBuilder)).DeepCopy ();
					Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Success, Translation.GetString ("NotificationInformation"), Translation.GetString ("ViewMyBuilderMessage1"));
				} else {
					Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("ViewMyBuilderError1"));
				}
			} catch (Exception err) {
				Global.ShowNotification (Plugin.Toasts.ToastNotificationType.Error, Translation.GetString ("NotificationError"), Translation.GetString ("ViewMyBuilderError2"));
			}
			theStack.BindingContext = builder;
			theAI.IsRunning = false;
			theAI.IsVisible = false;
			btValidate.IsEnabled = true;
		}

		void EntryLogo_TextChanged (object sender, TextChangedEventArgs e)
		{
			try {
				imgLogo.Source = ImageSource.FromUri (new Uri (entryLogo.Text.Trim ()));
			} catch (Exception) {
				imgLogo.Source = null;
			}
		}

	}
}