using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AstroBuilders
{

	[ContentProperty ("BackgroundColor")]
	public class ColorBgResourceExtension : IMarkupExtension
	{
		public string BackgroundColor { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (BackgroundColor == null)
				return Global.ColorBackground;
			switch (BackgroundColor) {
			case "Box":
				return Global.ColorBoxBackground;
			case "Border":
				return Global.ColorBoxBorder;
			case "MiniBorder":
				return Global.ColorBoxMiniBorder;
			case "Back2":
				return Global.Color2Background;
			case "Box2":
				return Global.Color2BoxBackground;
			default:
				return Global.ColorBackground;
			}
		}
	}

	[ContentProperty ("TextColor")]
	public class ColorTextResourceExtension : IMarkupExtension
	{
		public string TextColor { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (TextColor == null)
				return Global.ColorText;
			switch (TextColor) {
			case "Box":
				return Global.ColorBoxText;
			case "BoxHigh":
				return Global.ColorBoxHighText;
			case "BoxLow":
				return Global.ColorBoxLowText;
			case "High":
				return Global.ColorHighText;
			case "Box2":
				return Global.Color2BoxText;
			case "High2":
				return Global.Color2BoxHighText;
			default:
				return Global.ColorText;
			}
		}
	}

}