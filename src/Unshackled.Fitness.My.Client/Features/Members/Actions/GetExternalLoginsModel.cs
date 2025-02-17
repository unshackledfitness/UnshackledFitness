using MediatR;
using Unshackled.Fitness.My.Client.Features.Members.Models;

namespace Unshackled.Fitness.My.Client.Features.Members.Actions;

public class GetExternalLoginsModel
{
	public class Query : IRequest<ExternalLoginsModel> { }

	public class Handler : BaseMemberHandler, IRequestHandler<Query, ExternalLoginsModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ExternalLoginsModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<ExternalLoginsModel>($"{baseUrl}get-external-logins") ?? new();
		}
	}
}
