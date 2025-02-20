using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class StartWorkout
{
	public class Command : IRequest<CommandResult<DateTimeOffset>>
	{
		public string WorkoutSid { get; private set; }

		public Command(string workoutSid)
		{
			WorkoutSid = workoutSid;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Command, CommandResult<DateTimeOffset>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<DateTimeOffset>> Handle(Command request, CancellationToken cancellationToken)
		{
			var startTime = DateTime.Now;
			StartWorkoutModel model = new()
			{
				DateStarted = startTime,
				DateStartedUtc = startTime.ToUniversalTime(),
				WorkoutSid = request.WorkoutSid
			};

			return await PostToCommandResultAsync<StartWorkoutModel, DateTimeOffset>($"{baseUrl}start", model)
				?? new CommandResult<DateTimeOffset>(false, Globals.UnexpectedError);
		}
	}
}
