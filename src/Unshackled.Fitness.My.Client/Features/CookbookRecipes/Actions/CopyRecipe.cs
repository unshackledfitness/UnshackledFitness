using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Actions;

public class CopyRecipe
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormCopyRecipeModel Model { get; private set; }

		public Command(FormCopyRecipeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCookbookRecipeHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormCopyRecipeModel, string>($"{baseUrl}copy", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
