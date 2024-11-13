using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;

public class SearchTemplatesModel : SearchModel
{
	public string? Title { get; set; }
	public string? ActivityTypeSid { get; set; }
}
