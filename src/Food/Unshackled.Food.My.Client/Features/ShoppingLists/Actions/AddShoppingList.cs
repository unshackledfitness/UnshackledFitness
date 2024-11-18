using MediatR;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class AddShoppingList
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormShoppingListModel Model { get; private set; }

		public Command(FormShoppingListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormShoppingListModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}