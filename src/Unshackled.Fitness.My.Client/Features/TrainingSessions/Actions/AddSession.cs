using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;

public class AddSession
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormSessionModel Model { get; private set; }

		public Command(FormSessionModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseTrainingSessionHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormSessionModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}