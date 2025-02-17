using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

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
