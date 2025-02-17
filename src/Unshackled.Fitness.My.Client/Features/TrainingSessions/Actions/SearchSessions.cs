using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;

public class SearchSessions
{
	public class Query : IRequest<SearchResult<SessionListItem>>
	{
		public SearchSessionsModel Model { get; private set; }

		public Query(SearchSessionsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingSessionHandler, IRequestHandler<Query, SearchResult<SessionListItem>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<SessionListItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchSessionsModel, SearchResult<SessionListItem>>($"{baseUrl}search", request.Model) ??
				new SearchResult<SessionListItem>();
		}
	}
}
