using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

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
