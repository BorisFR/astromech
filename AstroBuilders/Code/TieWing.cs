using System;
using Xamarin.Forms;

namespace AstroBuilders
{
	public class TieWing : Label
	{
		public const string Typeface = "TIE-Wing";  

		public TieWing()
		{
			FontAttributes = FontAttributes.None;
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