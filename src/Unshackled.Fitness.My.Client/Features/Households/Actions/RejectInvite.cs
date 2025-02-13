using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public class RejectInvite
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
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}reject-invite", request.HouseholdSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
