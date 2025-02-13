using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Unshackled.Fitness.My.Extensions;
public static class ImageExtensions
{
	public static byte[] ResizeJpegTo(this byte[] srcImage, int width, double aspectRatio)
	{
		var image = Image.Load(srcImage);
		int height = (int)Math.Floor(width / aspectRatio);
		int jpgQuality = 60;

		if (image.Width >= width || image.Height >= height)
		{

			ResizeOptions resize = new()
			{
				Mode = ResizeMode.Crop,
				Position = AnchorPositionMode.Center,
				Size = new Size(width, height)
			};

			using Image cloneImage = image.Clone(x => x.Resize(resize));
			using var outStream = new MemoryStream();
			cloneImage.Save(outStream, new JpegEncoder()
			{
				Quality = jpgQuality
			});
			return outStream.ToArray();
		}

		return srcImage;
	}
}
