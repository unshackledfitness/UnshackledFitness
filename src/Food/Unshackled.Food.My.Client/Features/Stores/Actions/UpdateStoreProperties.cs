using MediatR;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Actions;

public class UpdateStoreProperties
{
	public class Command : IRequest<CommandResult<StoreModel>>
	{
		public FormStoreModel Model { get; private set; }

		public Command(FormStoreModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult<StoreModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<StoreModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormStoreModel, StoreModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<StoreModel>(false, Globals.UnexpectedError);
		}
	}
}
