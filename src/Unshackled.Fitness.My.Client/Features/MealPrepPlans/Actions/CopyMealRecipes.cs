using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class CopyMealRecipes
{
	public class Command : IRequest<CommandResult>
	{
		public CopyRecipesModel Model { get; private set; }

		public Command(CopyRecipesModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}copy-recipes", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}