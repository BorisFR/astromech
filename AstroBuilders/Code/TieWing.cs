using System;
using Xamarin.Forms;
using Plugin.DeviceInfo.Abstractions;

namespace AstroBuilders
{
	public class TieWing : Label
	{
		public const string Typeface = "TIE-Wing";

		public TieWing ()
		{
			FontAttributes = FontAttributes.None;
			if (Plugin.DeviceInfo.CrossDeviceInfo.Current.Platform == Platform.Windows)
				FontFamily = @"\Assets\TIE-Wing.ttf#TIE-Wing";
			else
				FontFamily = Typeface; 
		}

		/*
		protected override void OnPropertyChanged (string propertyName)
		{
			FontAttributes = FontAttributes.None;
			FontFamily = Typeface;
			base.OnPropertyChanged (propertyName);
		}
		*/

	}
}