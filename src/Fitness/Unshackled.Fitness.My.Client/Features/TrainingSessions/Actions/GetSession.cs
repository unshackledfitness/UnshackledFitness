using MediatR;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;

public class GetSession
{
	public class Query : IRequest<TrainingSessionModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseTrainingSessionHandler, IRequestHandler<Query, TrainingSessionModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<TrainingSessionModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<TrainingSessionModel>($"{baseUrl}get/{request.Sid}") ??
				new TrainingSessionModel();
		}
	}
}
