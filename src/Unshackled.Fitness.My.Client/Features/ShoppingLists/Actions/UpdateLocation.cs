using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.ShoppingLists.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ShoppingLists.Actions;

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