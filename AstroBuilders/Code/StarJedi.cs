using System;
using Xamarin.Forms;

namespace AstroBuilders
{
	public class StarJedi : Label
	{
		public const string Typeface = "Star Jedi";  

		public StarJedi()
		{
			FontAttributes = FontAttributes.None;
            if (DeviceInfo.Plugin.CrossDeviceInfo.Current.Platform == DeviceInfo.Plugin.Abstractions.Platform.Windows)
                FontFamily = @"\Assets\Star Jedi.ttf#Star Jedi";
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