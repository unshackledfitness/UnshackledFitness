using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class UpdateSlot
{
	public class Command : IRequest<CommandResult>
	{
		public SlotModel Model { get; private set; }

		public Command(SlotModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-slot", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}