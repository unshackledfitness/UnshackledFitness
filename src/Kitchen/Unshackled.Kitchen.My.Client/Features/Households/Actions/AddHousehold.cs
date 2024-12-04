using MediatR;
using Unshackled.Kitchen.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Households.Actions;

public class AddHousehold
{
	public class Command : IRequest<CommandResult<HouseholdListModel>>
	{
		public FormHouseholdModel Model { get; private set; }

		public Command(FormHouseholdModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult<HouseholdListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<HouseholdListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormHouseholdModel, HouseholdListModel>($"{baseUrl}add", request.Model)
				?? new CommandResult<HouseholdListModel>(false, Globals.UnexpectedError);
		}
	}
}
