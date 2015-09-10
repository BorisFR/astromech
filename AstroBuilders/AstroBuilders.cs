using System;

using Xamarin.Forms;
using AstroBuildersModel;
using Newtonsoft.Json;
using System.Reflection;

namespace AstroBuilders
{
	public class App : Application
	{
		public App ()
		{
            //var assembly = typeof(App).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames())
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
            Global.DoInit ();
			Global.MainAppPage = new MainAppPage ();
			MainPage = Global.MainAppPage;
        }

		protected override void OnStart ()
		{
            
			Global.DoInit ();
			// Handle when your app starts
			string json = Helper.SettingsRead<string>("User", string.Empty);
			if (json.Length == 0)
				return;
			User user = JsonConvert.DeserializeObject<User> (json);
			if (user == null)
				return;
			Global.ConnectedUser = user;
			Global.IsConnected = true;
			//Global.Menus.Refresh ();
			// TODO: do a re-logging of the user
			Global.Menus.Refresh ();
             
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

