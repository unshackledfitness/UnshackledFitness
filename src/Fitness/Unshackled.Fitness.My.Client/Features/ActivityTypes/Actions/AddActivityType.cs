using MediatR;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTypes.Actions;

public class AddActivityType
{
	public class Command : IRequest<CommandResult>
	{
		public FormActivityTypeModel Model { get; private set; }

		public Command(FormActivityTypeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseActivityTypeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormActivityTypeModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}