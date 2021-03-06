﻿using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{
	public partial class ViewAbout : ContentView
	{
		private bool isDone = false;

		public ViewAbout ()
		{
			InitializeComponent ();
			theContent.Content = new AppearingText (Translation.GetString ("ViewAboutTitle"));
			theWebview.Source = "http://r2builders.diverstrucs.com/About.html";
			ShowInfo ();
		}

		private void ShowInfo ()
		{
			if (theStack == null || theStack.Children == null)
				return;
			if (isDone)
				return;
			isDone = true;
			Device.BeginInvokeOnMainThread (() => {
				double width = -1;
				double height = -1;
				try {
					width = bigStack.Width;
					height = bigStack.Height;
				} catch (Exception) {
				}
				theStack.Children.Insert (0, new Label () {
					TextColor = Global.ColorBoxText,
					FontSize = FontSizeResourceExtension.CacheFontSizeDefault,
					Text = string.Format ("{0} {1} ({2}) {3}x{4}", Helper.DeviceInfo.Platform, Helper.DeviceInfo.Version, Helper.DeviceInfo.Model, width, height)
				});
				theStack.Children.Insert (0, new Label () {
					TextColor = Global.ColorBoxText,
					FontSize = FontSizeResourceExtension.CacheFontSizeDefault,
					Text = string.Format ("Device Id: {0}", Helper.DeviceInfo.Id)
				});
				theStack.Children.Insert (0, new Label () {
					TextColor = Global.ColorBoxText,
					FontSize = FontSizeResourceExtension.CacheFontSizeDefault,
					Text = string.Format ("Application Id: {0}", Global.UniqueAppId)
				});
			});
		}

		protected override void OnSizeAllocated (double width, double height)
		{
			base.OnSizeAllocated (width, height);
			ShowInfo ();
		}
	}
}

