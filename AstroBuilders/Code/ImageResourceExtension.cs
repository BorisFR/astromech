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
            if (Device.OS == TargetPlatform.Windows)
            {
                try
                {
                    var imageSource = ImageSource.FromResource("AstroBuilders.Win.Images." + Source);
                    return imageSource;
                }
                catch (Exception err)
                {
                    System.Diagnostics.Debug.WriteLine("********** ERROR: " + err.Message);
                }
                return null;
            }
			try {
				var imageSource = ImageSource.FromResource("AstroBuilders.Images." + Source);
				return imageSource;
			} catch(Exception err) {
				System.Diagnostics.Debug.WriteLine ("********** ERROR: " + err.Message);
			}
			return null;
		}
	}
}