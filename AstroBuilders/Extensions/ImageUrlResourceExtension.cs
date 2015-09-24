using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace AstroBuilders
{
	[ContentProperty ("Source")]
	public class ImageUrlResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Source == null)
				return null;

			try {
				var imageSource = ImageSource.FromUri (new Uri (string.Format ("{0}Content/Clubs/R2-FR.png", Global.BaseUrl, Source)));
				return imageSource;
			} catch (Exception err) {
				System.Diagnostics.Debug.WriteLine ("********** ERROR: " + err.Message);
			}
			return null;
		}
	}
}