using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace AstroBuilders
{
	[ContentProperty ("Source")]
	public class TextResourceExtension : IMarkupExtension
	{
		public string Source { get; set; }

		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Source == null)
				return null;
			try
			{
				var text = Translation.GetString(Source);
				return text;
			}
			catch (Exception err)
			{
				System.Diagnostics.Debug.WriteLine("TextResourceExtension*** ProvideValue :" + err.Message);
				return Source;
			}
		}

	}
}