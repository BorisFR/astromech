using System;

using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public class App : Application
	{
		public App ()
		{
			Global.DoInit ();
			Global.MainAppPage = new MainAppPage ();
			MainPage = Global.MainAppPage;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
			User user = Helper.SettingsRead<User>("User", null);
			if (user == null)
				return;
			Global.ConnectedUser = user;
			Global.IsConnected = true;
			Global.Menus.Refresh ();
			// TODO: do a re-logging of the user
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

