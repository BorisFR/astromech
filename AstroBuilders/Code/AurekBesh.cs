using System;
using Xamarin.Forms;

namespace AstroBuilders
{
	public class AurekBesh : Label
	{
		public const string Typeface = "Aurek-Besh";  

		public AurekBesh()
		{
            if (DeviceInfo.Plugin.CrossDeviceInfo.Current.Platform == DeviceInfo.Plugin.Abstractions.Platform.Windows)
                FontFamily = @"\Assets\Aurek-Besh.ttf#Aurek-Besh";
            else
			FontFamily = Typeface;    //iOS is happy with this, Android needs a renderer to add ".ttf"
		}

		/*
		protected override void OnPropertyChanged (string propertyName)
		{
			//FontSize = Device.GetNamedSize (NamedSize.Micro, this);
			FontFamily = Typeface;
			base.OnPropertyChanged (propertyName);
		}
		*/


	}
}