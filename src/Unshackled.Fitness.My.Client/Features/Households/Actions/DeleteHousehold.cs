using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public class DeleteHousehold
{
	public class Command : IRequest<CommandResult>
	{
		public string HouseholdSid { get; private set; }

		public Command(string householdSid)
		{
			HouseholdSid = householdSid;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult>
	{
		AppState state = default!;

		public Handler(HttpClient httpClient, AppState stateContainer) : base(httpClient)
		{
			this.state = stateContainer;
		}

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var result = await PostToCommandResultAsync($"{baseUrl}delete", request.HouseholdSid)
				?? new CommandResult(false, Globals.UnexpectedError);

			if (result.Success)
			{
				var member = (Member)state.ActiveMember;
				if (member.ActiveHousehold != null && member.ActiveHousehold.HouseholdSid == request.HouseholdSid)
				{
					member.ActiveHousehold = null;
					state.SetActiveMember(member);
				}
			}

			return result;
		}
	}
}
