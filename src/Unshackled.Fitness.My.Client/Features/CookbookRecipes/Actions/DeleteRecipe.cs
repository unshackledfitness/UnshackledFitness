using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Actions;

public class DeleteRecipe
{
	public class Command : IRequest<CommandResult>
	{
		public string RecipeSid { get; private set; }

		public Command(string sid)
		{
			RecipeSid = sid;
		}
	}

	public class Handler : BaseCookbookRecipeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.RecipeSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
