﻿using System;
using Xamarin.Forms;
using AstroBuilders.Droid;
using AstroBuilders;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(AurekBesh), typeof(AurekBeshRenderer))]

namespace AstroBuilders.Droid
{
	public class AurekBeshRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement == null)
			{
				//The ttf in /Assets is CaseSensitive
				Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, AurekBesh.Typeface + ".ttf");
			}
		}
	}
}