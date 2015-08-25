using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Toasts.Forms.Plugin.Droid;
using Xamarin.Forms;

namespace AstroBuilders.Droid
{

	[Activity (Label = "AstroBuilders", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			Forms.SetTitleBarVisibility(AndroidTitleBarVisibility.Never);
			ToastNotificatorImplementation.Init();

			LoadApplication (new App ());
		}

		public override void OnAttachedToWindow ()
		{
			base.OnAttachedToWindow ();
			Android.Views.View x = Window.DecorView;
			x.SystemUiVisibility = StatusBarVisibility.Hidden;
			x.KeepScreenOn = true;
			Window.SetFlags (WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);
		}

	}
}