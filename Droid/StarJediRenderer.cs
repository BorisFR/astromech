using System;
using Xamarin.Forms;
using AstroBuilders.Droid;
using AstroBuilders;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(StarJedi), typeof(StarJediRenderer))]

namespace AstroBuilders.Droid
{
	public class StarJediRenderer : LabelRenderer
	{
		public static Typeface CacheStarJedi = Typeface.CreateFromAsset(Forms.Context.Assets, StarJedi.Typeface + ".ttf");

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			//if (e.OldElement == null)
			{
				//if(CacheStarJedi == null)
				//	CacheStarJedi = Typeface.CreateFromAsset(Forms.Context.Assets, StarJedi.Typeface + ".ttf");
				//The ttf in /Assets is CaseSensitive
				Control.Typeface = CacheStarJedi;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			Control.Typeface = CacheStarJedi;
		}

	}
}