using MediatR;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Actions;

public class UpdateLocationSorts
{
	public class Command : IRequest<CommandResult>
	{
		public UpdateSortsModel Model { get; private set; }

		public Command(UpdateSortsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-location-sorts", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
