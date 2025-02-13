using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class UpdateIngredients
{
	public class Command : IRequest<CommandResult>
	{
		public string RecipeSid { get; private set; }
		public UpdateIngredientsModel Model { get; private set; }

		public Command(string recipeSid, UpdateIngredientsModel model)
		{
			RecipeSid = recipeSid;
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update/{request.RecipeSid}/ingredients", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
