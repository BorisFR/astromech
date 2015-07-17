using System;

using Xamarin.Forms;

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

