using MediatR;
using Unshackled.Food.My.Client.Features.ShoppingLists.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ShoppingLists.Actions;

public class UpdateLocation
{
	public class Command : IRequest<CommandResult<FormListItemModel>>
	{
		public UpdateLocationModel Model { get; private set; }

		public Command(UpdateLocationModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseShoppingListHandler, IRequestHandler<Command, CommandResult<FormListItemModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<FormListItemModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<UpdateLocationModel, FormListItemModel>($"{baseUrl}update-product-location", request.Model)
				?? new CommandResult<FormListItemModel>(false, Globals.UnexpectedError);
		}
	}
}