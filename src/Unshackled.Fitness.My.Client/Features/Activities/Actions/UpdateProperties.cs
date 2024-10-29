using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Activities.Models;

namespace Unshackled.Fitness.My.Client.Features.Activities.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormActivityModel Model { get; private set; }

		public Command(FormActivityModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseActivityHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormActivityModel, string>($"{baseUrl}update", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}