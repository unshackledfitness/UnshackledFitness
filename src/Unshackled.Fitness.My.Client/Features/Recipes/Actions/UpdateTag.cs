using MediatR;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class UpdateTag
{
	public class Command : IRequest<CommandResult>
	{
		public FormTagModel Model { get; private set; }

		public Command(FormTagModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-tag", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}