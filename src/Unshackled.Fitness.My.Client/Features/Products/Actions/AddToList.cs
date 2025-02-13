using MediatR;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Actions;

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

	public class Handler : BaseProductHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-to-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}