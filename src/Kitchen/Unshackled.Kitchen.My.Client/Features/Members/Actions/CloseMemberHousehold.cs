using MediatR;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Members.Actions;

public class CloseMemberHousehold
{
	public class Command : IRequest<CommandResult>
	{
		public string HouseholdSid { get; private set; }

		public Command(string householdId)
		{
			HouseholdSid = householdId;
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
			var member = await PostToResultAsync<string, Member>($"{baseUrl}close-household", request.HouseholdSid);
			if (member != null)
			{
				state.SetActiveMember(member);
				return new CommandResult(true, "Household closed.");
			}
			return new CommandResult(false, "Could not close the household.");
		}
	}
}
