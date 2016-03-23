using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Plugin.Toasts;
using Xamarin.Forms;

namespace AstroBuilders.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			DependencyService.Register<ToastNotificatorImplementation> ();
			ToastNotificatorImplementation.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

