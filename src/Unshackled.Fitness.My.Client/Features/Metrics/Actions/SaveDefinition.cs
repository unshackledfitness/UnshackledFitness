using MediatR;
using Unshackled.Fitness.My.Client.Features.Metrics.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Metrics.Actions;

public class SaveDefinition
{
	public class Command : IRequest<CommandResult>
	{
		public FormMetricDefinitionModel Model { get; private set; }

		public Command(FormMetricDefinitionModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMetricHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}save-definition", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
