using MediatR;
using Unshackled.Fitness.My.Client.Features.ActivityTargets.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTargets.Actions;

public class SearchTargets
{
	public class Query : IRequest<SearchResult<TargetListItem>>
	{
		public SearchTargetsModel Model { get; private set; }

		public Query(SearchTargetsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseActivityTargetHandler, IRequestHandler<Query, SearchResult<TargetListItem>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<TargetListItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchTargetsModel, SearchResult<TargetListItem>>($"{baseUrl}search", request.Model) ??
				new SearchResult<TargetListItem>();
		}
	}
}
