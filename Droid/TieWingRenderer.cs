using System;
using Xamarin.Forms;
using AstroBuilders.Droid;
using AstroBuilders;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(TieWing), typeof(TieWingRenderer))]

namespace AstroBuilders.Droid
{
	public class TieWingRenderer : LabelRenderer
	{
		public static Typeface CacheTieWing = Typeface.CreateFromAsset(Forms.Context.Assets, TieWing.Typeface + ".ttf");

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			//if (e.OldElement == null)
			{
				//if(CacheTieWing == null)
				//	CacheTieWing = Typeface.CreateFromAsset(Forms.Context.Assets, TieWing.Typeface + ".ttf");
				//The ttf in /Assets is CaseSensitive
				Control.Typeface = CacheTieWing;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			Control.Typeface = CacheTieWing;
		}

	}
}