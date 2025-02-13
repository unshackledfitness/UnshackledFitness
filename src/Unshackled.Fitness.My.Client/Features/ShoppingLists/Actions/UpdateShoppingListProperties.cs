using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

public class UpdateShoppingListProperties
{
	public class Command : IRequest<CommandResult<ShoppingListModel>>
	{
		public FormShoppingListModel Model { get; private set; }

		public Command(FormShoppingListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult<ShoppingListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ShoppingListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormShoppingListModel, ShoppingListModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<ShoppingListModel>(false, Globals.UnexpectedError);
		}
	}
}
