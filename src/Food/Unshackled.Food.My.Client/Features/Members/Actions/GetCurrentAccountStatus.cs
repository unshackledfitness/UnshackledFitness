using MediatR;
using Unshackled.Food.My.Client.Features.Members.Models;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public class GetCurrentAccountStatus
{
	public class Query : IRequest<CurrentAccountStatusModel> { }

	public class Handler : BaseMemberHandler, IRequestHandler<Query, CurrentAccountStatusModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CurrentAccountStatusModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<CurrentAccountStatusModel>($"{baseUrl}get-account-status") ?? new();
		}
	}
}
