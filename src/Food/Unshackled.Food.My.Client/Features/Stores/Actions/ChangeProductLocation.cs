using MediatR;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Actions;

public class ChangeProductLocation
{
	public class Command : IRequest<CommandResult>
	{
		public ChangeLocationModel Model { get; private set; }

		public Command(ChangeLocationModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}change-product-location", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
