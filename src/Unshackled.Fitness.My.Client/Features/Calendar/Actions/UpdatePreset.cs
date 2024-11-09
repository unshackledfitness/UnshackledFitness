using MediatR;
using Unshackled.Fitness.My.Client.Features.Calendar.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Calendar.Actions;

public class UpdatePreset
{
	public class Command : IRequest<CommandResult<PresetModel>> 
	{
		public string PresetSid { get; private set; }
		public string Settings { get; private set; }

		public Command(string sid, string settings)
		{
			PresetSid = sid;
			Settings = settings;
		}
	}

	public class Handler : BaseCalendarHandler, IRequestHandler<Command, CommandResult<PresetModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<PresetModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<string, PresetModel>($"{baseUrl}update-preset/{request.PresetSid}", request.Settings)
				?? new CommandResult<PresetModel>(false, Globals.UnexpectedError);
		}
	}
}
