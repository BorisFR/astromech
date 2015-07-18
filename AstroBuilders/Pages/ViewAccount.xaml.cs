using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;
using Newtonsoft.Json;

namespace AstroBuilders
{
	public partial class ViewAccount : ContentView
	{

		private bool clickInProgress = false;
		private string noFlag = "aucun";

		public ViewAccount ()
		{
			InitializeComponent ();

			btLogin.Clicked += BtLogin_Clicked;
			btDisconnect.Clicked += BtDisconnect_Clicked;
			btCreate.Clicked += BtCreate_Clicked;
			btModify.Clicked += BtModify_Clicked;
			btChangePassword.Clicked += BtChangePassword_Clicked;
			btForget.Clicked += BtForget_Clicked;

			pickerCreateCountry.Items.Add (noFlag);
			pickerModifyCountry.Items.Add (noFlag);
			foreach (Country c in Global.AllCountry.Collection) {
				pickerCreateCountry.Items.Add (c.Title);
				pickerModifyCountry.Items.Add (c.Title);
			}

			pickerCreateCountry.SelectedIndexChanged += PickerCreateCountry_SelectedIndexChanged;
			pickerModifyCountry.SelectedIndexChanged += PickerModifyCountry_SelectedIndexChanged;

			AdaptDisplay ();
		}

		void PickerModifyCountry_SelectedIndexChanged (object sender, EventArgs e)
		{
			Picker p = (Picker)sender;
			if (p.SelectedIndex == -1) {
				modifyCountry.Source = null;
				return;
			}
			string s = p.Items [p.SelectedIndex];
			if (s.Equals (noFlag)) {
				modifyCountry.Source = null;
				return;
			}
			Country country = null;
			foreach(Country c in Global.AllCountry.Collection) {
				if(c.Title.Equals(s)) {
					country = c;
					break;
				}
			}
			modifyCountry.Source = ImageSource.FromUri(new Uri(string.Format("{0}{1}",Global.BaseUrl, country.Flag)));
		}

		void PickerCreateCountry_SelectedIndexChanged (object sender, EventArgs e)
		{
			Picker p = (Picker)sender;
			if (p.SelectedIndex == -1) {
				createCountry.Source = null;
				return;
			}
			string s = p.Items [p.SelectedIndex];
			if (s.Equals (noFlag)) {
				createCountry.Source = null;
				return;
			}
			Country country = null;
			foreach(Country c in Global.AllCountry.Collection) {
				if(c.Title.Equals(s)) {
					country = c;
					break;
				}
			}
			createCountry.Source = ImageSource.FromUri(new Uri(string.Format("{0}{1}",Global.BaseUrl, country.Flag)));
		}

		void BtForget_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			if (entryLogin.Text != null)
				entryLogin.Text = entryLogin.Text.Trim ();
			if (string.IsNullOrEmpty (entryLogin.Text)) {
				//Global.MainAppPage.DisplayAlert ("Erreur", "Pour initialiser le processus de récupération du mot de passe, la saisie de l'identifiant est obligatoire.", "Ok");
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Pour initialiser le processus de récupération du mot de passe, la saisie de l'identifiant est obligatoire.");
				clickInProgress = false;
				return;
			}
			// TODO: forget password
			Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Info, "Information", "Désolé, cette fonctionnalité n'est pas encore implémentée.");
			clickInProgress = false;
		}

		void BtChangePassword_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			if (string.IsNullOrEmpty(entryOldPassword.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryNewPassword.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryNew2Password.Text)){
				clickInProgress = false;
				return;
			}
			entryOldPassword.Text = entryOldPassword.Text.Trim ();
			entryNewPassword.Text = entryNewPassword.Text.Trim ();
			entryNew2Password.Text = entryNew2Password.Text.Trim ();
			if (string.IsNullOrEmpty(entryOldPassword.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryNewPassword.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryNew2Password.Text)){
				clickInProgress = false;
				return;
			}
			if (!entryOldPassword.Text.Equals(Global.ConnectedUser.Password)) {
				//Global.MainAppPage.DisplayAlert ("Erreur", "Le mot de passe n'est pas valide.", "Ok");
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Le mot de passe n'est pas valide.");
				clickInProgress = false;
				return;
			}
			if (!entryNewPassword.Text.Equals(entryNew2Password.Text)) {
				//Global.MainAppPage.DisplayAlert ("Erreur", "La saisie du nouveau mot de passe ne correspond pas.", "Ok");
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "La saisie du nouveau mot de passe ne correspond pas.");
				clickInProgress = false;
				return;
			}
			// TODO: change password
			Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Info, "Information", "Désolé, cette fonctionnalité n'est pas encore implémentée.");
			clickInProgress = false;
		}

		void BtModify_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			//entryModifyNickname.Text = entryModifyNickname.Text.Trim ();
			if (string.IsNullOrEmpty (entryModifyPassword.Text)) {
				clickInProgress = false;
				return;
			}
			entryModifyPassword.Text = entryModifyPassword.Text.Trim ();
			//if (string.IsNullOrEmpty(entryModifyNickname.Text))
			//	return;
			if (string.IsNullOrEmpty (entryModifyPassword.Text)) {
				clickInProgress = false;
				return;
			}
			if (!entryModifyPassword.Text.Equals(Global.ConnectedUser.Password)) {
				//Global.MainAppPage.DisplayAlert ("Erreur", "Le mot de passe n'est pas valide.", "Ok");
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Le mot de passe n'est pas valide.");
				clickInProgress = false;
				return;
			}
			User user = Global.ConnectedUser;
			if (pickerModifyCountry.SelectedIndex == -1 || pickerModifyCountry.Items [pickerModifyCountry.SelectedIndex].Equals (noFlag)) {
				//user.IdCountry = Guid.Empty;
			} else {
				string s = pickerModifyCountry.Items [pickerModifyCountry.SelectedIndex];
				Country country = null;
				foreach(Country c in Global.AllCountry.Collection) {
					if(c.Title.Equals(s)) {
						country = c;
						break;
					}
				}
				user.IdCountry = country.Id;
			}

			user.Email = entryModifyEmail.Text.Trim ();
			Tools.JobDone += Tools_UpdateDone;
			Tools.DoUpdateUser (user);
		}

		void Tools_UpdateDone (bool status)
		{
			Tools.JobDone -= Tools_UpdateDone;
			try {
				if (status) {
					string json = Helper.Decrypt (Tools.Result);
					User user = JsonConvert.DeserializeObject<User> (json);
					Global.ConnectedUser = user;
					entryModifyPassword.Text = string.Empty;
					Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Info, "Information", "Vos modifications ont bien été enregistrées.");
				}
			} catch (Exception) {
			}
			clickInProgress = false;
			Global.Menus.Refresh ();
			AdaptDisplay ();
		}

		/*
		private void DoLogin() {
			Global.ConnectedUser = new User () { IdBuilder = new Guid("35190659-6da3-4ce5-b94a-6601e7a21f38"), NickName = "Boris"};
			Global.IsConnected = true;
			clickInProgress = false;
			Global.Menus.Refresh ();
			AdaptDisplay ();
		}
		*/
		void BtCreate_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			if (string.IsNullOrEmpty(entryCreateLogin.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryCreatePassword.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryCreateNickname.Text)){
				clickInProgress = false;
				return;
			}
			entryCreateLogin.Text = entryCreateLogin.Text.Trim ();
			entryCreatePassword.Text = entryCreatePassword.Text.Trim ();
			entryCreateNickname.Text = entryCreateNickname.Text.Trim ();
			if (entryCreateEmail.Text != null)
				entryCreateEmail.Text = entryCreateEmail.Text.Trim ();
			if (string.IsNullOrEmpty(entryCreateLogin.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryCreatePassword.Text)){
				clickInProgress = false;
				return;
			}
			if (string.IsNullOrEmpty(entryCreateNickname.Text)){
				clickInProgress = false;
				return;
			}
			clickInProgress = true;
			btCreate.IsEnabled = false;

			User user = new User();
			user.Login = entryCreateLogin.Text;
			user.Password = entryCreatePassword.Text;
			user.NickName = entryCreateNickname.Text;
			user.Email = entryCreateEmail.Text;
			if (pickerCreateCountry.SelectedIndex == -1 || pickerCreateCountry.Items [pickerCreateCountry.SelectedIndex].Equals (noFlag)) {
				user.IdCountry = Guid.Empty;
			} else {
				string s = pickerCreateCountry.Items [pickerCreateCountry.SelectedIndex];
				Country country = null;
				foreach(Country c in Global.AllCountry.Collection) {
					if(c.Title.Equals(s)) {
						country = c;
						break;
					}
				}
				user.IdCountry = country.Id;
			}
			Tools.JobDone += Tools_CreateDone;
			Tools.DoCreateUser (user);
		}

		void Tools_CreateDone (bool status) {
			Tools.JobDone -= Tools_CreateDone;
			if (!status) {
				btCreate.IsEnabled = true;
				clickInProgress = false;
				return;
			}
			try {
				string json = Helper.Decrypt (Tools.Result);
				User user = JsonConvert.DeserializeObject<User> (json);
				if (user.Title.StartsWith ("§")) {
					string message = string.Empty;
					message = "Les informations saisies sont erronées. Merci de corriger votre saisie.";
					if (user.Title.StartsWith ("§1")) {
						message = "Nous sommes désolé mais cet identifiant est déjà existant. Merci d'en choisir un autre.";
					}
					if (user.Title.StartsWith ("§2")) {
						message = "Nous sommes désolé mais ce nom d'utilisateur est déjà existant. Merci d'en choisir un autre.";
					}
					/*
					Device.BeginInvokeOnMainThread (() => {
						Global.MainAppPage.DisplayAlert ("Erreur", message, "Ok");
						btCreate.IsEnabled = true;
						clickInProgress = false;
						return;
					});
					*/
					Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", message);
					btCreate.IsEnabled = true;
					clickInProgress = false;
					return;
				}
				entryCreateLogin.Text = string.Empty;
				entryCreatePassword.Text = string.Empty;
				entryCreateNickname.Text = string.Empty;
				entryCreateEmail.Text = string.Empty;
				Global.ConnectedUser = user;
				Global.IsConnected = true;
				Global.Menus.Refresh ();
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Success, "Bienvenue", "Votre compte utilisateur a été créé.");
			} catch (Exception) {
				/*
				message = "Les informations saisies sont erronées. Merci de corriger votre saisie.";
				Device.BeginInvokeOnMainThread (() => {
					Global.MainAppPage.DisplayAlert("Erreur", message, "Ok");
					btLogin.IsEnabled = true;
					clickInProgress = false;
					return;
				});
				*/
			}
			btCreate.IsEnabled = true;
			clickInProgress = false;
			AdaptDisplay ();
		}

		void BtDisconnect_Clicked (object sender, EventArgs e)
		{
			Global.IsConnected = false;
			Global.ConnectedUser = null;
			Global.Menus.Refresh ();
			AdaptDisplay ();
			Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Info, "Information", "Vous êtes maintenant déconnecté de l'application.");
		}

		private void AdaptDisplay() {
			if (Global.IsConnected) {
				lConnexion.IsVisible = false;
				lConnected.IsVisible = true;
				lUser.Text = Global.ConnectedUser.NickName;
				//entryModifyNickname.Text = Global.ConnectedUser.NickName;
				if (string.IsNullOrEmpty (Global.ConnectedUser.Email))
					entryModifyEmail.Text = string.Empty;
				else
					entryModifyEmail.Text = Global.ConnectedUser.Email;
				if (Global.ConnectedUser.IdCountry.Equals (Guid.Empty))
					pickerModifyCountry.SelectedIndex = 0;
				else {
					Country country = (Country)Global.AllCountry.GetByGuid<Country> (Global.ConnectedUser.IdCountry);
					for (int i = 1; i < pickerModifyCountry.Items.Count; i++) {
						if (pickerModifyCountry.Items [i].Equals (country.Title)) {
							pickerModifyCountry.SelectedIndex = i;
							break;
						}
					}
				}
			} else {
				lConnexion.IsVisible = true;
				lConnected.IsVisible = false;
			}
		}

		void BtLogin_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			if (string.IsNullOrEmpty(entryLogin.Text))
				return;
			if (string.IsNullOrEmpty(entryPassword.Text))
				return;
			entryLogin.Text = entryLogin.Text.Trim ();
			entryPassword.Text = entryPassword.Text.Trim ();
			if (string.IsNullOrEmpty(entryLogin.Text))
				return;
			if (string.IsNullOrEmpty(entryPassword.Text))
				return;

			clickInProgress = true;
			btLogin.IsEnabled = false;

			User user = new User();
			user.Login = entryLogin.Text;
			user.Password = entryPassword.Text;
			Tools.JobDone += Tools_JobDone;
			Tools.DoCheckUser (user);
		}

		void Tools_JobDone (bool status)
		{
			Tools.JobDone -= Tools_JobDone;
			if (!status) {
				btLogin.IsEnabled = true;
				clickInProgress = false;
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Une erreur est survenue lors de votre authentification. Veuillez réessayer.");
				return;
			}
			try {
				string json = Helper.Decrypt (Tools.Result);
				User user = JsonConvert.DeserializeObject<User> (json);
				if(user.Title.StartsWith("§")) {
					string message = string.Empty;
					message = "Les informations saisies sont erronées. Merci de corriger votre saisie.";
					/*
					Device.BeginInvokeOnMainThread (() => {
						Global.MainAppPage.DisplayAlert("Erreur", message, "Ok");
						btLogin.IsEnabled = true;
						clickInProgress = false;
						return;
					});
					*/
					Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", message);
					btLogin.IsEnabled = true;
					clickInProgress = false;
					return;
				}
				/*
				if(user.Login.Equals("b")) {
					user.IsAdmin = true;
					user.IsModo = true;
				}*/
				Helper.SettingsSave<User>("User", user);
				entryLogin.Text = string.Empty;
				entryPassword.Text = string.Empty;
				entryOldPassword.Text = string.Empty;
				entryNewPassword.Text = string.Empty;
				entryNew2Password.Text = string.Empty;
				Global.ConnectedUser = user;
				btLogin.IsEnabled = true;
				Global.IsConnected = true;
				clickInProgress = false;
				Global.Menus.Refresh ();
				AdaptDisplay ();
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Info, "Bienvenue", "Vous êtes maintenant identifié.");
			} catch (Exception) {
				/*
				message = "Les informations saisies sont erronées. Merci de corriger votre saisie.";
				Device.BeginInvokeOnMainThread (() => {
					Global.MainAppPage.DisplayAlert("Erreur", message, "Ok");
					btLogin.IsEnabled = true;
					clickInProgress = false;
					return;
				});
				*/
				Global.ShowNotification(Toasts.Forms.Plugin.Abstractions.ToastNotificationType.Error, "Erreur", "Vous n'avez pas été reconnu. Merci de corriger les informations saisies.");
			}
			btLogin.IsEnabled = true;
			clickInProgress = false;
		}

	}
}