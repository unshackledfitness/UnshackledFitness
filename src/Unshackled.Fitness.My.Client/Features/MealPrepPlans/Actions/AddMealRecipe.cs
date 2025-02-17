using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.MealPrepPlans.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.MealPrepPlans.Actions;

public class AddMealRecipe
{
	public class Command : IRequest<CommandResult<MealPrepPlanRecipeModel>>
	{
		public AddPlanRecipeModel Model { get; private set; }

		public Command(AddPlanRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPrepPlanHandler, IRequestHandler<Command, CommandResult<MealPrepPlanRecipeModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<MealPrepPlanRecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<AddPlanRecipeModel, MealPrepPlanRecipeModel>($"{baseUrl}add-recipe", request.Model)
				?? new CommandResult<MealPrepPlanRecipeModel>(false, Globals.UnexpectedError);
		}
	}
}