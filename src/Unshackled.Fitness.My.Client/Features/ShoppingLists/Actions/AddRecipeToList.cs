using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public class AddRecipeToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddRecipesToListModel Model { get; private set; }

		public Command(AddRecipesToListModel model)
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
