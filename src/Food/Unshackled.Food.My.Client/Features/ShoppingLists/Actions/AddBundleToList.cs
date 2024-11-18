using MediatR;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class AddBundleToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddProductBundleModel Model { get; private set; }

		public Command(AddProductBundleModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-bundle-to-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}