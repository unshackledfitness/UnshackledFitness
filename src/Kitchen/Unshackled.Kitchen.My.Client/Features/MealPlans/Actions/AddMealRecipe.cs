using MediatR;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;

public class AddMealRecipe
{
	public class Command : IRequest<CommandResult<MealPlanRecipeModel>>
	{
		public AddPlanRecipeModel Model { get; private set; }

		public Command(AddPlanRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult<MealPlanRecipeModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<MealPlanRecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<AddPlanRecipeModel, MealPlanRecipeModel>($"{baseUrl}add-recipe", request.Model)
				?? new CommandResult<MealPlanRecipeModel>(false, Globals.UnexpectedError);
		}
	}
}