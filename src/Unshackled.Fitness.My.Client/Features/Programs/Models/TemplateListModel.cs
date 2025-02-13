using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Programs.Models;

public class TemplateListModel : BaseObject
{
	public string Title { get; set; } = string.Empty;
	public string? MusclesTargeted { get; set; }
}
