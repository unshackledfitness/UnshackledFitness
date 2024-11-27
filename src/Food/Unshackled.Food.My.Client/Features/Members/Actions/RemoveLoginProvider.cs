using MediatR;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public class RemoveLoginProvider
{
	public class Command : IRequest<CommandResult>
	{
		public FormProviderModel Model { get; private set; }

		public Command(FormProviderModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMemberHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}remove-login-provider", request.Model) 
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
