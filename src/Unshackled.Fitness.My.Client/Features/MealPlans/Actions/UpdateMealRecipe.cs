using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPlans.Actions;

public class UpdateMealRecipe
{
	public class Command : IRequest<CommandResult>
	{
		public MealPlanRecipeModel Model { get; private set; }

		public Command(MealPlanRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-recipe", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}