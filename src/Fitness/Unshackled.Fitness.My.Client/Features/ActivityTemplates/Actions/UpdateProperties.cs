using MediatR;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates.Actions;

public class UpdateProperties
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormTemplateModel Model { get; private set; }

		public Command(FormTemplateModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseActivityTemplateHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormTemplateModel, string>($"{baseUrl}update", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}