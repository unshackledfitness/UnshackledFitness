using MediatR;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public class UpdateRecipeProperties
{
	public class Command : IRequest<CommandResult<RecipeModel>>
	{
		public FormRecipeModel Model { get; private set; }

		public Command(FormRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult<RecipeModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<RecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormRecipeModel, RecipeModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<RecipeModel>(false, Globals.UnexpectedError);
		}
	}
}
