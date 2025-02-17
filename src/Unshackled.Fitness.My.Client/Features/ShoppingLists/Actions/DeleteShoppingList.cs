using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public class DeleteShoppingList
{
	public class Command : IRequest<CommandResult>
	{
		public string ShoppingListSid { get; private set; }

		public Command(string shoppingListSid)
		{
			ShoppingListSid = shoppingListSid;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.ShoppingListSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
