using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public class DeleteMember
{
	public class Command : IRequest<CommandResult>
	{
		public string HouseholdSid { get; private set; }
		public string MemberSid { get; private set; }

		public Command(string householdSid, string memberSid)
		{
			HouseholdSid = householdSid;
			MemberSid = memberSid;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete-from/{request.HouseholdSid}", request.MemberSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
