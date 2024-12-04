using MediatR;
using Unshackled.Kitchen.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Dashboard.Actions;

public class AddToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddToListModel Model { get; private set; }

		public Command(AddToListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseDashboardHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-to-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}