using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

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
