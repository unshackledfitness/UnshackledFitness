namespace Unshackled.Fitness.My.Client.Models;

public class AppStateConfig
{
	public string ContentBaseUrl { get; set; }
	public int MaxUploadInMb { get; set; }
	public string SiteName { get; set; }

	public AppStateConfig()
	{
		ContentBaseUrl = "/";
		MaxUploadInMb = 3;
		SiteName = string.Empty;
	}

	public AppStateConfig(string siteName, string contentBaseUrl, int maxUploadInMb)
	{
		ContentBaseUrl = contentBaseUrl;
		MaxUploadInMb = maxUploadInMb;
		SiteName = siteName;
	}
}
