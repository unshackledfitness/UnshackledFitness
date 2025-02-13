using Unshackled.Fitness.Core.Interfaces;

namespace Unshackled.Fitness.My.Client.Models;

public abstract class SearchModel : ISearchModel
{
	public const int DefaultPageSize = 25;
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = DefaultPageSize;
	public List<ISearchSortModel> Sorts { get; set; } = new();
	public int Skip => Page > 1 ? (Page - 1) * PageSize : 0;
}
