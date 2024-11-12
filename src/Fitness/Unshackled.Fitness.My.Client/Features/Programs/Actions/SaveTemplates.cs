using MediatR;
using Unshackled.Fitness.My.Client.Features.Programs.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Programs.Actions;

public class SaveTemplates
{
	public class Command : IRequest<CommandResult>
	{
		public FormUpdateTemplatesModel Model { get; private set; }

		public Command(FormUpdateTemplatesModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProgramHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}save-templates", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
