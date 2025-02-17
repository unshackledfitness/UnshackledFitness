using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public class UpdateMember
{
	public class Command : IRequest<CommandResult>
	{
		public FormMemberModel Model { get; private set; }

		public Command(FormMemberModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-member", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
