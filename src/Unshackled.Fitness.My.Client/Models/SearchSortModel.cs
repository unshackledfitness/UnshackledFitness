using Unshackled.Fitness.Core.Interfaces;

namespace Unshackled.Fitness.My.Client.Models;

public class SearchSortModel : ISearchSortModel
{
	public string Member { get; set; } = string.Empty;
	public int SortDirection { get; set; }
}
