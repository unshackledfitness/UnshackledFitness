using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.TrainingSessions.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.TrainingSessions.Actions;

public class DuplicateSession
{
	public class Command : IRequest<CommandResult<string>>
	{
		public string Sid { get; private set; }
		public FormSessionModel Model { get; private set; }

		public Command(string sid, FormSessionModel model)
		{
			Sid = sid;
			Model = model;
		}
	}

	public class Handler : BaseTrainingSessionHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormSessionModel, string>($"{baseUrl}duplicate/{request.Sid}", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
