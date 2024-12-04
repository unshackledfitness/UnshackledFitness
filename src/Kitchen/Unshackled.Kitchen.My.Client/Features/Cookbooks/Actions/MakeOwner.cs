using MediatR;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Cookbooks.Actions;

public class MakeOwner
{
	public class Command : IRequest<CommandResult>
	{
		public MakeOwnerModel Model { get; private set; }

		public Command(MakeOwnerModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}make-owner", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
