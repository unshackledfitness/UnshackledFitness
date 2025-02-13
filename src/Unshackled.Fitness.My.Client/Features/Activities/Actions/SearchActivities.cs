using MediatR;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Actions;

public class SearchActivities
{
	public class Query : IRequest<SearchResult<ActivityListModel>>
	{
		public SearchActivitiesModel Model { get; private set; }

		public Query(SearchActivitiesModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseActivityHandler, IRequestHandler<Query, SearchResult<ActivityListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<ActivityListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchActivitiesModel, SearchResult<ActivityListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<ActivityListModel>();
		}
	}
}
