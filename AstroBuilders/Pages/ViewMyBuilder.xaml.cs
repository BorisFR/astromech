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
			textClub.Text = string.Format ("Mon club : {0}", club.Title);
			logoClub.Source = club.Logo;

			entryLogo.TextChanged += EntryLogo_TextChanged;
			theStack.BindingContext = builder;

			btValidate.Clicked += BtValidate_Clicked;
		}

		void BtValidate_Clicked (object sender, EventArgs e)
		{
			theAI.IsRunning = true;
			theAI.IsVisible = true;
			Tools.JobDone += Tools_UpdateDone;
			Tools.DoUpdateBuilder (builder);
		}

		void Tools_UpdateDone (bool status) {
			Tools.JobDone -= Tools_UpdateDone;
			try {
				if (status) {
					string json = Helper.Decrypt (Tools.Result);
					Global.AllBuilders.LoadFromJson (json);
					builder = ((Builder)Global.AllBuilders.GetByGuid<Builder> (Global.ConnectedUser.IdBuilder)).DeepCopy ();
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Success, "Information", "La mise à jour a bien été effectuée.");
				} else {
					Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Un problème est survenue pendant la mise à jour.");
				}
			} catch (Exception err) {
				Global.ShowNotification (Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Une erreur est survenue pendant la mise à jour.");
			}
			theStack.BindingContext = builder;
			theAI.IsRunning = false;
			theAI.IsVisible = false;
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