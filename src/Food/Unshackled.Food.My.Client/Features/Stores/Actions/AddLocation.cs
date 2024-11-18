using MediatR;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Stores.Actions;

public class AddLocation
{
	public class Command : IRequest<CommandResult>
	{
		public FormStoreLocationModel Model { get; private set; }

		public Command(FormStoreLocationModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormStoreLocationModel, string>($"{baseUrl}add-location", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}