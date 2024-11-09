using MediatR;
using Unshackled.Fitness.My.Client.Features.Calendar.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Calendar.Actions;

public class AddPreset
{
	public class Command : IRequest<CommandResult<PresetModel>> 
	{
		public FormPresetModel Model { get; private set; }

		public Command(FormPresetModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCalendarHandler, IRequestHandler<Command, CommandResult<PresetModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<PresetModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormPresetModel, PresetModel>($"{baseUrl}add-preset", request.Model)
				?? new CommandResult<PresetModel>(false, Globals.UnexpectedError);
		}
	}
}
