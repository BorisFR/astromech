using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class ViewAbout : ContentView
	{
		public ViewAbout ()
		{
			InitializeComponent ();
			theWebview.Source = "http://r2builders.diverstrucs.com/About.html";

			theStack.Children.Insert (0, new Label () { TextColor=Global.ColorBoxText, FontSize=FontSizeResourceExtension.CacheFontSizeDefault, Text = string.Format ("{0} {1} ({2})", Helper.DeviceInfo.Platform, Helper.DeviceInfo.Version, Helper.DeviceInfo.Model) });
			theStack.Children.Insert (0, new Label () { TextColor=Global.ColorBoxText, FontSize=FontSizeResourceExtension.CacheFontSizeDefault, Text = string.Format ("Device Id: {0}", Helper.DeviceInfo.Id) });
			theStack.Children.Insert (0, new Label () { TextColor=Global.ColorBoxText, FontSize=FontSizeResourceExtension.CacheFontSizeDefault, Text = string.Format ("Application Id: {0}", Global.UniqueAppId) });
		}
	}
}

