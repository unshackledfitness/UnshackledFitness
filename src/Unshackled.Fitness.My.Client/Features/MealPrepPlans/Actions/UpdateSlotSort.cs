using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class UpdateSlotSort
{
	public class Command : IRequest<CommandResult>
	{
		public List<SlotModel> Definitions { get; private set; }

		public Command(List<SlotModel> definitions)
		{
			Definitions = definitions;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-slot-sort", request.Definitions)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}