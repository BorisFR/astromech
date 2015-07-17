using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class MainAppPage : MasterDetailPage
	{
		public MainAppPage ()
		{
			InitializeComponent ();

			Global.MenuPage = new MenuPage ();
			this.Master = Global.MenuPage;
			Global.DetailPage = new DetailPage ();
			this.Detail = Global.DetailPage;
		}
	}
}