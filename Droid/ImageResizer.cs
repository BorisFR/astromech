using System;
using Android.Graphics;
using System.IO;
using AstroBuilders.Droid;

[assembly: Xamarin.Forms.Dependency (typeof (ImageResizer))]

namespace AstroBuilders.Droid
{
	public static class ImageResizer : IImageResizer
	{
		public static byte[] ResizeImage (byte[] imageData, float width, float height)
		{
			// Load the bitmap
			Bitmap originalImage = BitmapFactory.DecodeByteArray (imageData, 0, imageData.Length);
			float oldWidth = (float)originalImage.Width;
			float oldHeight = (float)originalImage.Height;
			float scaleFactor = 0f;

			if (oldWidth > oldHeight)
			{
				scaleFactor = width / oldWidth;
			}
			else
			{
				scaleFactor = height / oldHeight;
			}

			float newHeight = oldHeight * scaleFactor;
			float newWidth = oldWidth * scaleFactor;

			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress (Bitmap.CompressFormat.Jpeg, 100, ms);
				return ms.ToArray ();
			}
		}

	}
}