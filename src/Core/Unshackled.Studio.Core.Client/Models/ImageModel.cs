using Unshackled.Studio.Core.Client.Enums;

namespace Unshackled.Studio.Core.Client.Models;

public class ImageModel
{
	public string Sid { get; set; } = string.Empty;
	public string Container { get; set; } = string.Empty;
	public string RelativePath { get; set; } = string.Empty;
	public bool IsFeatured { get; set; }

	public string ImageUrl(string baseUrl)
	{
		baseUrl = baseUrl.TrimEnd('/');
		string container = Container.TrimStart('/').TrimEnd('/');
		string relativePath = RelativePath.TrimStart('/');

		return string.Format("{0}/{1}/{2}", baseUrl, container, relativePath);
	}

	public static ImageModel Default()
	{
		return Default(AspectRatios.R16x9);
	}

	public static ImageModel Default(AspectRatios ratio)
	{
		return new ImageModel
		{
			Container = "content",
			RelativePath = $"placeholder-{ratio.Title()}.webp",
			IsFeatured = true
		};
	}
}
