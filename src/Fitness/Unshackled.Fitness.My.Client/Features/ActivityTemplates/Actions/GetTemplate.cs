using MediatR;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates.Actions;

public class GetTemplate
{
	public class Query : IRequest<ActivityTemplateModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseActivityTemplateHandler, IRequestHandler<Query, ActivityTemplateModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ActivityTemplateModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<ActivityTemplateModel>($"{baseUrl}get/{request.Sid}") ??
				new ActivityTemplateModel();
		}
	}
}
