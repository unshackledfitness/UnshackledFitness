using MediatR;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class BulkAddLocations
{
	public class Command : IRequest<CommandResult>
	{
		public FormBulkAddLocationModel Model { get; private set; }

		public Command(FormBulkAddLocationModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormBulkAddLocationModel, string>($"{baseUrl}bulk-add-locations", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}