using MediatR;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public class UpdateSteps
{
	public class Command : IRequest<CommandResult>
	{
		public string RecipeSid { get; private set; }
		public UpdateStepsModel Model { get; private set; }

		public Command(string recipeSid, UpdateStepsModel model)
		{
			RecipeSid = recipeSid;
			Model = model;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update/{request.RecipeSid}/steps", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
