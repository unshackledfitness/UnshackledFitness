using MediatR;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Actions;

public class GetTemplateForm
{
	public class Query : IRequest<FormActivityModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseActivityHandler, IRequestHandler<Query, FormActivityModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<FormActivityModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<FormActivityModel>($"{baseUrl}get-template-form/{request.Sid}") ??
				new FormActivityModel();
		}
	}
}
