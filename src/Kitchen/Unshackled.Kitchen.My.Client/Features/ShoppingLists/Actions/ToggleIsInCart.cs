using MediatR;
using Unshackled.Kitchen.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.ShoppingLists.Actions;

public class ToggleIsInCart
{
	public class Command : IRequest<CommandResult>
	{
		public ToggleListItemModel Model { get; private set; }

		public Command(ToggleListItemModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}toggle/in-cart", request.Model) ??
				new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
