using MediatR;
using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Members.Actions;

public class OpenMemberCookbook
{
	public class Command : IRequest<CommandResult>
	{
		public string CookbookSid { get; private set; }

		public Command(string cookbookId)
		{
			CookbookSid = cookbookId;
		}
	}

	public class Handler : BaseMemberHandler, IRequestHandler<Command, CommandResult>
	{
		AppState state = default!;

		public Handler(HttpClient httpClient, IAppState stateContainer) : base(httpClient)
		{
			this.state = (AppState)stateContainer;
		}

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var member = await PostToResultAsync<string, Member>($"{baseUrl}open-cookbook", request.CookbookSid);
			if (member != null)
			{
				state.SetActiveMember(member);
				return new CommandResult(true, "Cookbook opened.");
			}
			return new CommandResult(false, "Could not open the cookbook.");
		}
	}
}
