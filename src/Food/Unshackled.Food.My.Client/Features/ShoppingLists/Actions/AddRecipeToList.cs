using MediatR;
using Unshackled.Food.Core.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class AddRecipeToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddRecipeToListModel Model { get; private set; }

		public Command(AddRecipeToListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-recipe-to-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
