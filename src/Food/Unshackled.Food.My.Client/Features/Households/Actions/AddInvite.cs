using MediatR;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Households.Actions;

public class AddInvite
{
	public class Command : IRequest<CommandResult<InviteListModel>>
	{
		public string HouseholdSid { get; private set; }
		public FormAddInviteModel Model { get; private set; }

		public Command(string groupSid, FormAddInviteModel model)
		{
			HouseholdSid = groupSid;
			Model = model;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult<InviteListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<InviteListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormAddInviteModel, InviteListModel>($"{baseUrl}invite/{request.HouseholdSid}", request.Model)
				?? new CommandResult<InviteListModel>(false, Globals.UnexpectedError);
		}
	}
}
