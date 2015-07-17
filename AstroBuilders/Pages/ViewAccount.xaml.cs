using System;
using System.Collections.Generic;
using Xamarin.Forms;
using AstroBuildersModel;
using Newtonsoft.Json;

namespace AstroBuilders
{
	public partial class ViewAccount : ContentView
	{

		bool clickInProgress = false;

		public ViewAccount ()
		{
			InitializeComponent ();

			btLogin.Clicked += BtLogin_Clicked;
			btDisconnect.Clicked += BtDisconnect_Clicked;
			btCreate.Clicked += BtCreate_Clicked;
			btModify.Clicked += BtModify_Clicked;
			btChangePassword.Clicked += BtChangePassword_Clicked;
			btForget.Clicked += BtForget_Clicked;
			AdaptDisplay ();
		}

		void BtForget_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			entryLogin.Text = entryLogin.Text.Trim ();
			if (string.IsNullOrEmpty (entryLogin.Text)) {
				Global.MainAppPage.DisplayAlert ("Erreur", "Pour initialiser le processus de récupération du mot de passe, la saisie de l'identifiant est obligatoire.", "Ok");
				return;
			}
			// TODO: forget password
		}

		void BtChangePassword_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			entryOldPassword.Text = entryOldPassword.Text.Trim ();
			entryNewPassword.Text = entryNewPassword.Text.Trim ();
			entryNew2Password.Text = entryNew2Password.Text.Trim ();
			if (string.IsNullOrEmpty(entryOldPassword.Text))
				return;
			if (string.IsNullOrEmpty(entryNewPassword.Text))
				return;
			if (string.IsNullOrEmpty(entryNew2Password.Text))
				return;
			if (!entryOldPassword.Text.Equals(Global.ConnectedUser.Password)) {
				Global.MainAppPage.DisplayAlert ("Erreur", "Le mot de passe n'est pas valide.", "Ok");
				return;
			}
			if (!entryNewPassword.Text.Equals(entryNew2Password.Text)) {
				Global.MainAppPage.DisplayAlert ("Erreur", "La saisie du nouveau mot de passe ne correspond pas.", "Ok");
				return;
			}
			// TODO: change password
		}

		void BtModify_Clicked (object sender, EventArgs e)
		{
			if (clickInProgress)
				return;
			entryModifyNickname.Text = entryModifyNickname.Text.Trim ();
			entryModifyPassword.Text = entryModifyPassword.Text.Trim ();
			if (string.IsNullOrEmpty(entryModifyNickname.Text))
				return;
			if (string.IsNullOrEmpty(entryModifyPassword.Text))
				return;
			if (!entryModifyPassword.Text.Equals(Global.ConnectedUser.Password)) {
				Global.MainAppPage.DisplayAlert ("Erreur", "Le mot de passe n'est pas valide.", "Ok");
				return;
			}
			// TODO: change account (nickname and email)
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
			if (string.IsNullOrEmpty(entryCreateLogin.Text))
				return;
			if (string.IsNullOrEmpty(entryCreatePassword.Text))
				return;
			if (string.IsNullOrEmpty(entryCreateNickname.Text))
				return;
			entryCreateLogin.Text = entryCreateLogin.Text.Trim ();
			entryCreatePassword.Text = entryCreatePassword.Text.Trim ();
			entryCreateNickname.Text = entryCreateNickname.Text.Trim ();
			entryCreateEmail.Text = entryCreateEmail.Text.Trim ();
			if (string.IsNullOrEmpty(entryCreateLogin.Text))
				return;
			if (string.IsNullOrEmpty(entryCreatePassword.Text))
				return;
			if (string.IsNullOrEmpty(entryCreateNickname.Text))
				return;
			clickInProgress = true;
			btCreate.IsEnabled = false;

			User user = new User();
			user.Login = entryCreateLogin.Text.Trim();
			user.Password = entryCreatePassword.Text;
			user.NickName = entryCreateNickname.Text;
			user.Email = entryCreateEmail.Text;
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
					Device.BeginInvokeOnMainThread (() => {
						Global.MainAppPage.DisplayAlert ("Erreur", message, "Ok");
						btCreate.IsEnabled = true;
						clickInProgress = false;
						return;
					});
					return;
				}
				entryCreateLogin.Text = string.Empty;
				entryCreatePassword.Text = string.Empty;
				entryCreateNickname.Text = string.Empty;
				entryCreateEmail.Text = string.Empty;
				Global.ConnectedUser = user;
				btCreate.IsEnabled = true;
				Global.IsConnected = true;
				clickInProgress = false;
				Global.Menus.Refresh ();
				AdaptDisplay ();
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
		}

		void BtDisconnect_Clicked (object sender, EventArgs e)
		{
			Global.IsConnected = false;
			Global.ConnectedUser = null;
			Global.Menus.Refresh ();
			AdaptDisplay ();
		}

		private void AdaptDisplay() {
			if (Global.IsConnected) {
				lConnexion.IsVisible = false;
				lConnected.IsVisible = true;
				lUser.Text = Global.ConnectedUser.NickName;
				entryModifyNickname.Text = Global.ConnectedUser.NickName;
				if (string.IsNullOrEmpty (Global.ConnectedUser.Email))
					entryModifyEmail.Text = string.Empty;
				else
					entryModifyEmail.Text = Global.ConnectedUser.Email;
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
				return;
			}
			try {
				string json = Helper.Decrypt (Tools.Result);
				User user = JsonConvert.DeserializeObject<User> (json);
				if(user.Title.StartsWith("§")) {
					string message = string.Empty;
					message = "Les informations saisies sont erronées. Merci de corriger votre saisie.";
					Device.BeginInvokeOnMainThread (() => {
						Global.MainAppPage.DisplayAlert("Erreur", message, "Ok");
						btLogin.IsEnabled = true;
						clickInProgress = false;
						return;
					});
				}
				/*
				if(user.Login.Equals("b")) {
					user.IsAdmin = true;
					user.IsModo = true;
				}*/
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
			btLogin.IsEnabled = true;
			clickInProgress = false;
		}

	}
}