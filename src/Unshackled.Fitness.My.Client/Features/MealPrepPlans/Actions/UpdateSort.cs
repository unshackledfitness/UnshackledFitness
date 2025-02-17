
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class UpdateSort
{
	public class Command : IRequest<CommandResult>
	{
		public List<UpdateSortModel> Updates { get; private set; }

		public Command(List<UpdateSortModel> updates)
		{
			Updates = updates;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-sort", request.Updates)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}