using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Members.Actions;

public class GetActiveMember
{
	public class Query : IRequest<Unit> { }

	public class Handler : BaseMemberHandler, IRequestHandler<Query, Unit>
	{
		private readonly AppState state = default!;

		public Handler(HttpClient httpClient, AppState state) : base(httpClient)
		{
			this.state = state;
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
