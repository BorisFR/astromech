using System;
using Xamarin.Forms;
using System.Globalization;

namespace AstroBuilders
{
	public class StringToImageSourceUrlConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return null;
			string url = string.Format ("{0}TheImage/{1}", Global.BaseUrl, (value as string).Replace (".jpg", ""));
			System.Diagnostics.Debug.WriteLine ("Image: " + url);
			return ImageSource.FromUri (new Uri (url));
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

	}
}