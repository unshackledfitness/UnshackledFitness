using MediatR;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Households.Actions;

public class JoinHousehold
{
	public class Command : IRequest<CommandResult<HouseholdListModel>>
	{
		public string HouseholdSid { get; private set; }

		public Command(string groupSid)
		{
			HouseholdSid = groupSid;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult<HouseholdListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<HouseholdListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<string, HouseholdListModel>($"{baseUrl}join", request.HouseholdSid)
				?? new CommandResult<HouseholdListModel>(false, Globals.UnexpectedError);
		}
	}
}
