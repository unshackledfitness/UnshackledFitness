using MediatR;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class UpdateNotes
{
	public class Command : IRequest<CommandResult>
	{
		public FormNotesModel Model { get; private set; }

		public Command(FormNotesModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-notes", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
