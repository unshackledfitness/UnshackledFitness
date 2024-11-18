using MediatR;
using Unshackled.Food.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Households.Actions;

public class UpdateHouseholdProperties
{
	public class Command : IRequest<CommandResult<HouseholdModel>>
	{
		public FormHouseholdModel Model { get; private set; }

		public Command(FormHouseholdModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult<HouseholdModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<HouseholdModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormHouseholdModel, HouseholdModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<HouseholdModel>(false, Globals.UnexpectedError);
		}
	}
}
