using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class AddSlot
{
	public class Command : IRequest<CommandResult<List<SlotModel>>>
	{
		public SlotModel Model { get; private set; }

		public Command(SlotModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult<List<SlotModel>>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<List<SlotModel>>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<SlotModel, List<SlotModel>>($"{baseUrl}add-slot", request.Model)
				?? new CommandResult<List<SlotModel>>(false, Globals.UnexpectedError);
		}
	}
}