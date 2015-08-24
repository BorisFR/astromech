using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace AstroBuilders
{
	[ContentProperty ("Height")]
	public class FontSizeHeightResourceExtension : IMarkupExtension
	{
		public string Height { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Height == null)
				return null;

			switch (Height.Trim ().ToLower ()) {
			case "large":
				return FontSizeResourceExtension.CacheFontSizeLarge * 1.4;
			case "medium":
				return FontSizeResourceExtension.CacheFontSizeMedium * 1.3;
			case "small":
				return FontSizeResourceExtension.CacheFontSizeSmall * 1.2;
			case "micro":
				return FontSizeResourceExtension.CacheFontSizeMicro * 1.1;
			case "default":
				return FontSizeResourceExtension.CacheFontSizeDefault * 1.3;
			default:
				return FontSizeResourceExtension.CacheFontSizeDefault * 1.3;
			}

		}

	}
}