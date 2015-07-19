using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class DetailPage : ContentPage
	{
		public DetailPage ()
		{
			InitializeComponent ();
			ShowPage (MyPage.Home);
		}

		public void ShowPage(MyPage page) {
			switch (page) {
			case MyPage.Home:
				theFrame.Content = null;
				theFrame.Content = new ViewHome ();
				break;
			case MyPage.Builders:
				theFrame.Content = null;
				theFrame.Content = new ViewBuilders ();
				break;
			case MyPage.Account:
				theFrame.Content = null;
				theFrame.Content = new ViewAccount ();
				break;
			case MyPage.MyBuilder:
				theFrame.Content = null;
				theFrame.Content = new ViewMyBuilder ();
				break;
			case MyPage.AdminUsers:
				theFrame.Content = null;
				theFrame.Content = new ViewAdminUsers ();
				break;
			case MyPage.AdminBuilders:
				theFrame.Content = null;
				theFrame.Content = new ViewAdminBuilders ();
				break;
			case MyPage.About:
				theFrame.Content = null;
				theFrame.Content = new ViewAbout ();
				break;
			}
		}

	}
}