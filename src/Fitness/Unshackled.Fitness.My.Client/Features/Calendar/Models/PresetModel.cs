using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Calendar.Models;

public class PresetModel : BaseMemberObject
{
	public string Title { get; set; } = string.Empty;
	public string Settings { get; set; } = string.Empty;
}
