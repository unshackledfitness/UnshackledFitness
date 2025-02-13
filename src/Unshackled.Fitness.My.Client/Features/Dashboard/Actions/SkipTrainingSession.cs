using MediatR;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Actions;

public class SkipTrainingSession
{
	public class Command : IRequest<CommandResult<ScheduledListModel>>
	{
		public string PlanSid { get; private set; }

		public Command(string planSid)
		{
			PlanSid = planSid;
		}
	}

	public class Handler : BaseDashboardHandler, IRequestHandler<Command, CommandResult<ScheduledListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ScheduledListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<string, ScheduledListModel>($"{baseUrl}skip-training-session", request.PlanSid)
				?? new CommandResult<ScheduledListModel>(false, Globals.UnexpectedError);
		}
	}
}
