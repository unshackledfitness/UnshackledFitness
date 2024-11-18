using MediatR;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class AddProductsToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddProductsModel Model { get; private set; }

		public Command(AddProductsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-products-to-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}