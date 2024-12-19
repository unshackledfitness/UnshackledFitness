namespace Unshackled.Studio.Core.Client.Configuration;

public class StorageSettings
{
	public string BaseUrl { get; set; } = string.Empty;
	public string Container { get; set; } = string.Empty;
	public int MaxUploadInMb { get; set; } = 3;

	public string GetAbsoluteUrl(string relativeUrl)
	{
		string absUrl = BaseUrl;

		if (!absUrl.EndsWith("/"))
			absUrl += "/";

		absUrl += Container;

		if (!absUrl.EndsWith("/"))
			absUrl += "/";

		absUrl += relativeUrl; 
		
		if (!absUrl.EndsWith("/"))
			absUrl += "/";

		return absUrl;
	}
}
