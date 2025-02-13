using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Models;

public class SearchStoreModel : SearchModel
{
	public string? Title { get; set; }
	public bool IsArchived { get; set; }
}
