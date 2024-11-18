using MediatR;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public class SetTheme
{
	public class Command : IRequest<CommandResult>
	{
		public Themes Theme { get; private set; }

		public Command(Themes theme)
		{
			Theme = theme;
		}
	}

	public class Handler : BaseMemberHandler, IRequestHandler<Command, CommandResult>
	{
		private readonly AppState state = default!;

		public Handler(HttpClient httpClient, AppState state) : base(httpClient)
		{
			this.state = state;
		}

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var result = await PostToCommandResultAsync<Themes, Member>($"{baseUrl}set-theme", request.Theme)
				?? new CommandResult<Member>(false, Globals.UnexpectedError);

			if (result.Success && result.Payload != null)
			{
				state.SetActiveMember(result.Payload);
			}

			return new CommandResult(result.Success, result.Message);
		}
	}
}
