using MediatR;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

public class AddToCookbook
{
	public class Command : IRequest<CommandResult>
	{
		public AddToCookbookModel Model { get; private set; }

		public Command(AddToCookbookModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-to-cookbook", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
