using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace AstroBuilders
{
	[ContentProperty ("FontSize")]
	public class FontSizeResourceExtension : IMarkupExtension
	{

		public static double CacheFontSizeLarge = 0.0;
		public static double CacheFontSizeMedium = 0.0;
		public static double CacheFontSizeSmall = 0.0;
		public static double CacheFontSizeMicro = 0.0;
		public static double CacheFontSizeDefault = 0.0;

		public static double GetLargeValue {
			get { 
				if (CacheFontSizeLarge > 0.0)
					return CacheFontSizeLarge;
				CacheFontSizeLarge = Device.GetNamedSize (NamedSize.Large, typeof(Label));
				System.Diagnostics.Debug.WriteLine ("*** FontSizeLarge=" + CacheFontSizeLarge.ToString ());
				switch (Helper.DeviceInfo.Platform) {
				case DeviceInfo.Plugin.Abstractions.Platform.iOS:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						CacheFontSizeLarge = 18;
						return CacheFontSizeLarge;
					case TargetIdiom.Tablet:
						return CacheFontSizeLarge;
					case TargetIdiom.Desktop:
						return CacheFontSizeLarge;
					default:
						return CacheFontSizeLarge;
					}
				default:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						return CacheFontSizeLarge;
					case TargetIdiom.Tablet:
						return CacheFontSizeLarge;
					case TargetIdiom.Desktop:
						return CacheFontSizeLarge;
					default:
						return CacheFontSizeLarge;
					}
				}

			}
		}

		public static double GetMediumValue {
			get { 
				if (CacheFontSizeMedium > 0.0)
					return CacheFontSizeMedium;
				CacheFontSizeMedium = Device.GetNamedSize (NamedSize.Medium, typeof(Label));
				System.Diagnostics.Debug.WriteLine ("*** FontSizeMedium=" + CacheFontSizeMedium.ToString ());
				switch (Helper.DeviceInfo.Platform) {
				case DeviceInfo.Plugin.Abstractions.Platform.iOS:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						CacheFontSizeMedium = 14;
						return CacheFontSizeMedium;
					case TargetIdiom.Tablet:
						return CacheFontSizeMedium;
					case TargetIdiom.Desktop:
						return CacheFontSizeMedium;
					default:
						return CacheFontSizeMedium;
					}
				default:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						return CacheFontSizeMedium;
					case TargetIdiom.Tablet:
						return CacheFontSizeMedium;
					case TargetIdiom.Desktop:
						return CacheFontSizeMedium;
					default:
						return CacheFontSizeMedium;
					}
				}

			}
		}

		public static double GetSmallValue {
			get { 
				if (CacheFontSizeSmall > 0.0)
					return CacheFontSizeSmall;
				CacheFontSizeSmall = Device.GetNamedSize (NamedSize.Small, typeof(Label));
				System.Diagnostics.Debug.WriteLine ("*** FontSizeSmall=" + CacheFontSizeSmall.ToString ());
				switch (Helper.DeviceInfo.Platform) {
				case DeviceInfo.Plugin.Abstractions.Platform.iOS:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						CacheFontSizeSmall = 12;
						return CacheFontSizeSmall;
					case TargetIdiom.Tablet:
						return CacheFontSizeSmall;
					case TargetIdiom.Desktop:
						return CacheFontSizeSmall;
					default:
						return CacheFontSizeSmall;
					}
				default:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						return CacheFontSizeSmall;
					case TargetIdiom.Tablet:
						return CacheFontSizeSmall;
					case TargetIdiom.Desktop:
						return CacheFontSizeSmall;
					default:
						return CacheFontSizeSmall;
					}
				}

			}
		}

		public static double GetMicroValue {
			get { 
				if (CacheFontSizeMicro > 0.0)
					return CacheFontSizeMicro;
				CacheFontSizeMicro = Device.GetNamedSize (NamedSize.Micro, typeof(Label));
				System.Diagnostics.Debug.WriteLine ("*** FontSizeMicro=" + CacheFontSizeMicro.ToString ());
				switch (Helper.DeviceInfo.Platform) {
				case DeviceInfo.Plugin.Abstractions.Platform.iOS:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						CacheFontSizeMicro = 10;
						return CacheFontSizeMicro;
					case TargetIdiom.Tablet:
						return CacheFontSizeMicro;
					case TargetIdiom.Desktop:
						return CacheFontSizeMicro;
					default:
						return CacheFontSizeMicro;
					}
				default:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						return CacheFontSizeMicro;
					case TargetIdiom.Tablet:
						return CacheFontSizeMicro;
					case TargetIdiom.Desktop:
						return CacheFontSizeMicro;
					default:
						return CacheFontSizeMicro;
					}
				}

			}
		}

		public static double GetDefaultValue {
			get { 
				if (CacheFontSizeDefault > 0.0)
					return CacheFontSizeDefault;
				CacheFontSizeDefault = Device.GetNamedSize (NamedSize.Default, typeof(Label));
				System.Diagnostics.Debug.WriteLine ("*** FontSizeDefault=" + CacheFontSizeDefault);
				//System.Diagnostics.Debug.WriteLine ("*** FontSizeDefault=" + CacheFontSizeDefault.ToString ());
				System.Diagnostics.Debug.WriteLine ("*** Device.Idiom=" + Device.Idiom);
				switch (Helper.DeviceInfo.Platform) {
				case DeviceInfo.Plugin.Abstractions.Platform.iOS:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						CacheFontSizeDefault = 14;
						return CacheFontSizeDefault;
					case TargetIdiom.Tablet:
						return CacheFontSizeDefault;
					case TargetIdiom.Desktop:
						return CacheFontSizeDefault;
					default:
						return CacheFontSizeDefault;
					}
				default:
					switch (Device.Idiom) {
					case TargetIdiom.Phone:
						return CacheFontSizeDefault;
					case TargetIdiom.Tablet:
						return CacheFontSizeDefault;
					case TargetIdiom.Desktop:
						return CacheFontSizeDefault;
					default:
						return CacheFontSizeDefault;
					}
				}

			}
		}

		public string FontSize { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (FontSize == null)
				return GetDefaultValue;
			switch (FontSize.Trim ().ToLower ()) {
			case "large":
				return GetLargeValue;
			case "medium":
				return GetMediumValue;
			case "small":
				return GetSmallValue;
			case "micro":
				return GetMicroValue;
			default:
				return GetDefaultValue;
			}
		}

	}
}