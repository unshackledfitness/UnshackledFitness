using MediatR;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class SearchWorkouts
{
	public class Query : IRequest<SearchResult<WorkoutListModel>>
	{
		public SearchWorkoutModel Model { get; private set; }

		public Query(SearchWorkoutModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Query, SearchResult<WorkoutListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<WorkoutListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchWorkoutModel, SearchResult<WorkoutListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<WorkoutListModel>();
		}
	}
}
