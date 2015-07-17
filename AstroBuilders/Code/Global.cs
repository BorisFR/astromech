using System;
using Xamarin.Forms;
using AstroBuildersModel;

namespace AstroBuilders
{
	public enum MyPage
	{
		None,
		Home,
		Builders,
		Account,
		AdminBuilders,
		About
	}

	public static class Global
	{
		public static readonly Thickness PagePadding = new Thickness(Device.OnPlatform(0, 0, 0), Device.OnPlatform(20, 0, 0), Device.OnPlatform(0, 0, 0), Device.OnPlatform(0, 0, 0));
		public static string BaseUrl = "http://r2builders.diverstrucs.com/";

		public static MainAppPage MainAppPage;
		public static MenuPage MenuPage;
		public static DetailPage DetailPage;
		public static MenuManager Menus;

		public static NewsManager AllNews = new NewsManager ();
		public static BuildersManager AllBuilders = new BuildersManager ();
		public static ClubsManager AllClubs = new ClubsManager ();
		public static UsersManager AllUsers = new UsersManager ();

		public static Builder CurrentBuilder = null;

		public static bool IsConnected = false;
		public static User ConnectedUser = null;

		public static IFiles Files = null;

		public static void DoInit() {
			Files = DependencyService.Get<IFiles> ();
			Tools.DoInit ();
			Menus = new MenuManager ();
			Menus.Refresh ();

			IDataServer x = new IDataServer ("news");
			x.DataRefresh +=  delegate(bool status) {
				System.Diagnostics.Debug.WriteLine("Status: " + x.FileName + "=" + status);
				if(!status)
					return;
				AllNews.LoadFromJson(Helper.Decrypt(x.JsonData));
				AllNews.Refresh();
			};
			DataServer.AddToDo (x);

			IDataServer xx = new IDataServer ("builders");
			xx.DataRefresh +=  delegate(bool status) {
				System.Diagnostics.Debug.WriteLine("Status: " + xx.FileName + "=" + status);
				if(!status)
					return;
				AllBuilders.LoadFromJson(Helper.Decrypt(xx.JsonData));
			};
			DataServer.AddToDo (xx);

			IDataServer xxx = new IDataServer ("clubs");
			xxx.DataRefresh +=  delegate(bool status) {
				System.Diagnostics.Debug.WriteLine("Status: " + xxx.FileName + "=" + status);
				if(!status)
					return;
				AllClubs.LoadFromJson(Helper.Decrypt(xxx.JsonData));
			};
			DataServer.AddToDo (xxx);

		}

		public static void GotoPage(MyPage page) {
			AwesomeWrappanel.CleanAll ();
			DetailPage.ShowPage (page);
		}
	}
}