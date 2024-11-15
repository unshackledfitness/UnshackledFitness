using MediatR;
using Unshackled.Fitness.My.Client.Features.Programs.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Programs.Actions;

public class AddProgram
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormAddProgramModel Model { get; private set; }

		public Command(FormAddProgramModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProgramHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormAddProgramModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
