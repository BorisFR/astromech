using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace AstroBuilders
{
	[ContentProperty ("Source")]
	public class ImageResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Source == null)
				return null;

			try {
				ImageSource imageSource = ImageSource.FromResource("AstroBuilders.images." + Source);
				return imageSource;
			} catch(Exception err) {
				System.Diagnostics.Debug.WriteLine ("ERROR: " + err.Message);
			}
			return null;
		}
	}
}