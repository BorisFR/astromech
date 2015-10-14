using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AstroBuilders
{

	public class OneStar
	{
		public double X;
		public double Y;
		public double Z;

		public double ScreenX;
		public double ScreenY;
		public double ScreenSize;
		public int ScreenShade;

		public double ScreenOldX;
		public double ScreenOldY;
		public int ScreenOldSize;

		public Color TheColor{ get { return Color.FromRgb (ScreenShade, ScreenShade, ScreenShade); } }
	}

	public class Field
	{
		// inspired by
		// http://codentronix.com/2011/07/22/html5-canvas-3d-starfield/

		private const double MAXDEPTH = 32.0;

		public OneStar[] allStar = null;

		private double OneRandom (double min, double max)
		{
			return Global.Random.Next (Convert.ToInt32 (max - min)) + min;
		}

		public void CreateAllStar (int number)
		{
			allStar = new OneStar[number];
			for (int i = 0; i < number; i++) {
				allStar [i] = new OneStar ();
				allStar [i].X = OneRandom (-25, 25);
				allStar [i].Y = OneRandom (-25, 25);
				allStar [i].Z = OneRandom (1, MAXDEPTH);
			}
		}

		double k = 0;

		public void MoveAll (double width, double height)
		{
			double halfWidth = width / 2;
			double halfHeight = height / 2;
			foreach (OneStar o in allStar) {
				o.Z -= 0.2;
				if (o.Z <= 0) {
					o.X = OneRandom (-25, 25);
					o.Y = OneRandom (-25, 25);
					o.Z = MAXDEPTH;
				}
				k = 128.0 / o.Z;
				o.ScreenX = o.X * k + halfWidth;
				o.ScreenY = o.Y * k + halfHeight;
				o.ScreenSize = (1.0 - (o.Z / 32.0)) * 5.0;
				o.ScreenShade = Convert.ToInt32 ((1.0 - o.Z / MAXDEPTH) * 255);
			}
		}

	}

	public partial class DetailPage : ContentPage
	{
		public const int MAXSTARS = 100;

		Field field = null;
		BoxView[] allR = new BoxView[MAXSTARS];

		public DetailPage ()
		{
			InitializeComponent ();
            
			if (Translation.IsTextReady)
				ShowPage (MyPage.Home);
			else
				ShowPage (MyPage.FirstLoading);
			//Tools.Trace ("DetailPage done.");
			NavigationPage.SetHasNavigationBar (this, false);

			if (Device.OS != TargetPlatform.Android) {
				field = new Field ();
				field.CreateAllStar (MAXSTARS);
				for (int i = 0; i < MAXSTARS; i++) {
					allR [i] = new BoxView ();
					allR [i].Color = Color.Lime;
					allR [i].TranslationX = field.allStar [i].ScreenX;
					allR [i].TranslationY = field.allStar [i].ScreenY;
					allR [i].WidthRequest = 1; //field.allStar [i].ScreenSize;
					allR [i].HeightRequest = 1; //field.allStar [i].ScreenSize;
					theAnimation.Children.Add (allR [i], Constraint.RelativeToParent ((parent) => {
						return 0;
					}), Constraint.RelativeToParent ((parent) => {
						return 0;
					}));
				}
				Device.StartTimer (new TimeSpan (0, 0, 0, 0, 30), DoAnimation);
			}
		}

		private bool DoAnimation ()
		{
			field.MoveAll (this.Width, this.Height);
			for (int i = 0; i < MAXSTARS; i++) {
				allR [i].TranslationX = field.allStar [i].ScreenX;
				allR [i].TranslationY = field.allStar [i].ScreenY;
				allR [i].Scale = field.allStar [i].ScreenSize;
				//allR [i].WidthRequest = field.allStar [i].ScreenSize;
				//allR [i].HeightRequest = field.allStar [i].ScreenSize;
				allR [i].Color = Color.FromRgb (field.allStar [i].ScreenShade, field.allStar [i].ScreenShade, field.allStar [i].ScreenShade);
			}
			return true;
		}

		public void ShowPage (MyPage page)
		{
			switch (page) {
			case MyPage.FirstLoading:
				theFrame.Content = null;
				theFrame.Content = new ViewFirstLoading ();
				break;
			case MyPage.Home:
				theFrame.Content = null;
				theFrame.Content = new ViewHome ();
				break;
			case MyPage.Builders:
				theFrame.Content = null;
				theFrame.Content = new ViewBuilders ();
				break;
			case MyPage.Exhibitions:
				theFrame.Content = null;
				theFrame.Content = new ViewExhibitions ();
				break;
			case MyPage.Account:
				theFrame.Content = null;
				theFrame.Content = new ViewAccount ();
				break;
			case MyPage.MyBuilder:
				theFrame.Content = null;
				theFrame.Content = new ViewMyBuilder ();
				break;
			case MyPage.MyExhibitions:
				theFrame.Content = null;
				theFrame.Content = new ViewMyExhibitions ();
				break;
			case MyPage.AdminUsers:
				theFrame.Content = null;
				theFrame.Content = new ViewAdminUsers ();
				break;
			case MyPage.AdminBuilders:
				theFrame.Content = null;
				theFrame.Content = new ViewAdminBuilders ();
				break;
			case MyPage.About:
				theFrame.Content = null;
				theFrame.Content = new ViewAbout ();
				break;
			}
		}

	}
}