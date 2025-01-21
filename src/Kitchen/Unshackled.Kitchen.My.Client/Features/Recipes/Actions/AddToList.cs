using MediatR;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

public class AddToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddRecipesToListModel Model { get; private set; }

		public Command(AddRecipesToListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-to-shopping-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
