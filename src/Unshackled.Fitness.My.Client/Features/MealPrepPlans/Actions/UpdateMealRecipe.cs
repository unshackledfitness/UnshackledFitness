using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class UpdateMealRecipe
{
	public class Command : IRequest<CommandResult>
	{
		public MealPrepPlanRecipeModel Model { get; private set; }

		public Command(MealPrepPlanRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-recipe", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}