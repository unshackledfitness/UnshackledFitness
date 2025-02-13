using MediatR;
using Unshackled.Fitness.My.Client.Features.Workouts.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Workouts.Actions;

public class SaveSet
{
	public class Command : IRequest<CommandResult<FormWorkoutSetModel>>
	{
		public FormWorkoutSetModel Set { get; private set; }

		public Command(FormWorkoutSetModel set)
		{
			Set = set;
		}
	}

	public class Handler : BaseWorkoutHandler, IRequestHandler<Command, CommandResult<FormWorkoutSetModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<FormWorkoutSetModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormWorkoutSetModel, FormWorkoutSetModel>($"{baseUrl}save-set", request.Set)
				?? new CommandResult<FormWorkoutSetModel>(false, Globals.UnexpectedError);
		}
	}
}
