using MediatR;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Actions;

public class GetActivity
{
	public class Query : IRequest<ActivityModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseActivityHandler, IRequestHandler<Query, ActivityModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ActivityModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<ActivityModel>($"{baseUrl}get/{request.Sid}") ??
				new ActivityModel();
		}
	}
}
