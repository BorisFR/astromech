using System;
using Xamarin.Forms;

namespace AstroBuilders
{
	public class StarJedi : Label
	{
		public const string Typeface = "Star Jedi";  

		public StarJedi()
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