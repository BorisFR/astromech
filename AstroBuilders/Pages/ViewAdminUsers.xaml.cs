using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public partial class ViewAdminUsers : ContentView
	{

		User selectedUser = null;

		public ViewAdminUsers ()
		{
			InitializeComponent ();


			IDataServer x = new IDataServer ("users", true);
			x.DataRefresh +=  delegate(bool status) {
				System.Diagnostics.Debug.WriteLine("Status: " + x.FileName + "=" + status);
				if(!status)
					return;
				Global.AllUsers.LoadFromJson(Helper.Decrypt(x.JsonData));
				Device.BeginInvokeOnMainThread (() => {
					theUsers.ItemsSource = Global.AllUsers.Collection;
				});
			};
			DataServer.AddToDo (x);

			theUsers.ItemSelected += TheUsers_ItemSelected;
			panelUser.BindingContext = null;
			btValidate.Clicked += BtValidate_Clicked;
		}

		void BtValidate_Clicked (object sender, EventArgs e)
		{
			if (selectedUser == null)
				return;
			User current = (User)theUsers.SelectedItem;
			if (!current.Id.Equals (selectedUser.Id))
				return;
			if (current.IsAdmin == switchAdmin.IsToggled && current.IsBuilder == switchBuilder.IsToggled && current.IsModo == switchModo.IsToggled && current.IsNewser == switchNewser.IsToggled)
				return;
			if (switchBuilder.IsToggled && !current.IsBuilder) {
				current.IdBuilder = Guid.NewGuid ();
			}
			if (!switchBuilder.IsToggled && current.IsBuilder) {
				current.IdBuilder = Guid.Empty;
			}
			current.IsAdmin = switchAdmin.IsToggled;
			current.IsModo = switchModo.IsToggled;
			current.IsNewser = switchNewser.IsToggled;
			theAI.IsRunning = true;
			theAI.IsVisible = true;
			switchNewser.IsEnabled = false;
			switchAdmin.IsEnabled = false;
			switchBuilder.IsEnabled = false;
			switchModo.IsEnabled = false;
			btValidate.IsEnabled = false;
			Tools.JobDone += Tools_UpdateDone;
			Tools.DoUpdateUserByAdmin (current);
		}

		void Tools_UpdateDone (bool status) {
			Tools.JobDone -= Tools_UpdateDone;
			if (status) {
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Success, "Information", "La mise à jour a bien été effectuée.");
			} else {
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Un problème est survenue pendant la mise à jour.");
			}
			Device.BeginInvokeOnMainThread (() => {
				theUsers.ItemsSource = Global.AllUsers.Collection;
			});
			theUsers.SelectedItem = null;
			theAI.IsRunning = false;
			theAI.IsVisible = false;
			switchNewser.IsEnabled = true;
			switchAdmin.IsEnabled = true;
			switchBuilder.IsEnabled = true;
			switchModo.IsEnabled = true;
			btValidate.IsEnabled = true;
		}

		void TheUsers_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) {
				selectedUser = null;
				panelUser.BindingContext = null;
				return;
			}
			selectedUser = ((User)e.SelectedItem).DeepCopy ();
			panelUser.BindingContext = selectedUser;
			if (selectedUser.IsBuilder)
				switchBuilder.IsEnabled = false;
			else
				switchBuilder.IsEnabled = true;
		}

	}
}