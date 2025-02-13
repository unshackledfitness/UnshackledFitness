using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class StartWorkout
{
	public class Command : IRequest<CommandResult<DateTime>>
	{
		public string WorkoutSid { get; private set; }

		public Command(string workoutSid)
		{
			WorkoutSid = workoutSid;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Command, CommandResult<DateTime>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<DateTime>> Handle(Command request, CancellationToken cancellationToken)
		{
			var startTime = DateTime.Now;
			StartWorkoutModel model = new()
			{
				DateStarted = startTime,
				DateStartedUtc = startTime.ToUniversalTime(),
				WorkoutSid = request.WorkoutSid
			};

			return await PostToCommandResultAsync<StartWorkoutModel, DateTime>($"{baseUrl}start", model)
				?? new CommandResult<DateTime>(false, Globals.UnexpectedError);
		}
	}
}
