using MediatR;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products.Actions;

public class BulkArchiveRestore
{
	public class Command : IRequest<CommandResult>
	{
		public BulkArchiveModel Model { get; }

		public Command(BulkArchiveModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}bulk-archive-restore", request.Model) ??
				new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
