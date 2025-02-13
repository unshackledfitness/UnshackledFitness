namespace Unshackled.Fitness.Core.Interfaces;

public interface ISearchModel
{
	int Page { get; set; }
	int PageSize { get; set; }
	int Skip { get; }
	List<ISearchSortModel> Sorts { get; set; }
}
