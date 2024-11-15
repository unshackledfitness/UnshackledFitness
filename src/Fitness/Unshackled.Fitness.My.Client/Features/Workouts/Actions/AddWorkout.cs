using MediatR;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class AddWorkout
{
	public class Command : IRequest<CommandResult<string>>
	{
		public AddWorkoutModel Model { get; private set; }

		public Command(AddWorkoutModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<AddWorkoutModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
