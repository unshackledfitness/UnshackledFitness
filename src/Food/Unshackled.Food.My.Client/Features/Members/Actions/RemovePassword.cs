using MediatR;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public class RemovePassword
{
	public class Command : IRequest<CommandResult>
	{
		public FormRemovePasswordModel Model { get; private set; }

		public Command(FormRemovePasswordModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMemberHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete-password", request.Model) 
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
