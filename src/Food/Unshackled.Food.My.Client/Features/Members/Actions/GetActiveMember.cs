using MediatR;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public class GetActiveMember
{
	public class Query : IRequest<Unit> { }

	public class Handler : BaseMemberHandler, IRequestHandler<Query, Unit>
	{
		AppState state = default!;

		public Handler(HttpClient httpClient, IAppState stateContainer) : base(httpClient)
		{
			this.state = (AppState)stateContainer;
		}

		public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
		{
			var member = await GetResultAsync<Member>($"{baseUrl}active");
			if (member != null)
			{
				state.SetActiveMember(member);
			}
			else
			{
				state.SetMemberLoaded(true);
			}
			return Unit.Value;
		}
	}
}
