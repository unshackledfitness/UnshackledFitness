using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Stores.Models;

public class SearchStoreModel : SearchModel
{
	public string? Title { get; set; }
	public bool IsArchived { get; set; }
}
