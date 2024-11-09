using MediatR;
using Unshackled.Fitness.My.Client.Features.Programs.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Programs.Actions;

public class StartProgram
{
	public class Command : IRequest<CommandResult>
	{
		public FormStartProgramModel Model { get; private set; }

		public Command(FormStartProgramModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProgramHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}start", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
