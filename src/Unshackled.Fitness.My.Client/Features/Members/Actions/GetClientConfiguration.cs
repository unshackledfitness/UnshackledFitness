using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Members.Actions;

public class GetClientConfiguration
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
			var config = await GetResultAsync<AppStateConfig>($"{baseUrl}get-client-configuration");
			if (config != null)
			{
				state.SetConfiguration(config);
			}
			else
			{
				state.SetConfiguration(new AppStateConfig());
			}
			return Unit.Value;
		}
	}
}
