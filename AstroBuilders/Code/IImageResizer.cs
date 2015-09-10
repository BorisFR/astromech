using System;

namespace AstroBuilders
{
	public interface IImageResizer
	{
		byte[] ResizeImage (byte[] imageData, float width, float height);
	}
}