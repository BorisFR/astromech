using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AstroBuilders
{

	public enum TextAnimation
	{
		Wait,
		Appear,
		Disappear,
		AppearAurekBesh,
		DisappearAurekBesh
	}

	public partial class AppearingText : ContentView
	{

		private string theText = string.Empty;

		public string TheText {
			get { return theText; }
			set {
				theText = value;
				LaunchAnimation ();
			}
		}

		private Color theColor = Global.ColorText;
		private double theSize = FontSizeResourceExtension.GetLargeValue;
		private double theSize2 = FontSizeResourceExtension.GetSmallValue;

		private int currentPos = 0;
		private bool runningAnimation = false;
		private TextAnimation currentAnimation = TextAnimation.Appear;
		private int delayBeforeNextAnimation = 0;
		private int pauseAnimation = 0;

		public AppearingText ()
		{
			InitializeComponent ();

			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text)
		{
			InitializeComponent ();

			TheText = text;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, Color textColor)
		{
			InitializeComponent ();

			TheText = text;
			theColor = textColor;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, double fontSize, double fontSize2)
		{
			InitializeComponent ();

			TheText = text;
			theSize = fontSize;
			theSize2 = fontSize2;
			LaunchAnimation ();
			StartTimer ();
		}

		public AppearingText (string text, Color textColor, double fontSize, double fontSize2)
		{
			InitializeComponent ();

			TheText = text;
			theColor = textColor;
			theSize = fontSize;
			theSize2 = fontSize2;
			LaunchAnimation ();
			StartTimer ();
		}

		private void StartTimer ()
		{
			Device.StartTimer (new TimeSpan (0, 0, 0, 0, 80 + Global.Random.Next (40)), DoAnimation);
		}

		private void LaunchAnimation ()
		{
			runningAnimation = false;
			currentPos = 0;
			try {
				labelAnim.Text = string.Empty;
				labelText.Text = string.Empty;
				labelAnim.TextColor = theColor;
				labelText.TextColor = theColor;
				labelAnim.FontSize = theSize2;
				labelText.FontSize = theSize;
			} catch (Exception) {
			}
			runningAnimation = true;
		}

		private void ChooseRandomAnimation ()
		{
			switch (Global.Random.Next (2)) {
			case 0:
				currentAnimation = TextAnimation.Appear;
				break;
			case 1:
				currentAnimation = TextAnimation.AppearAurekBesh;
				break;
			}
		}

		private void DoPauseAnimation ()
		{
			pauseAnimation = Global.Random.Next (10) * 10;
		}

		private bool DoAnimation ()
		{
			if (!runningAnimation)
				return true;
			if (pauseAnimation > 0) {
				pauseAnimation--;
				return true;
			}
			try {

				switch (currentAnimation) {

				case TextAnimation.Wait:
					if (delayBeforeNextAnimation < 1) {
						delayBeforeNextAnimation = Global.Random.Next (3) * 10;
						ChooseRandomAnimation ();
					} else {
						delayBeforeNextAnimation--;
					}
					break;

				case TextAnimation.Appear:
					if (currentPos > theText.Length)
						return true;
					currentPos++;
					if (currentPos > 1)
						labelText.Text = theText.Substring (0, currentPos - 1);
					else
						labelText.Text = " ";
					if (currentPos <= theText.Length)
						labelAnim.Text = theText.Substring (currentPos - 1, 1);
					else {
						labelAnim.Text = " ";
						DoPauseAnimation ();
						currentAnimation = TextAnimation.Disappear;
					}
					break;

				case TextAnimation.AppearAurekBesh:
					if (currentPos > theText.Length)
						return true;
					currentPos++;
					if (currentPos <= theText.Length)
						labelAnim.Text = theText.Substring (0, currentPos);
					else {
						DoPauseAnimation ();
						currentAnimation = TextAnimation.DisappearAurekBesh;
					}
					break;

				case TextAnimation.Disappear:
					if (currentPos < 1)
						return true;
					currentPos--;
					if (currentPos > 1)
						labelText.Text = theText.Substring (0, currentPos - 1);
					else {
						labelText.Text = " ";
						currentAnimation = TextAnimation.Wait;
					}
					break;

				case TextAnimation.DisappearAurekBesh:
					if (currentPos < 1)
						return true;
					currentPos--;
					if (currentPos > 1)
						labelAnim.Text = theText.Substring (0, currentPos - 1);
					else {
						labelAnim.Text = " ";
						currentAnimation = TextAnimation.Wait;
					}
					break;

				}

			} catch (Exception) {
			}
			return true;
		}

	}
}