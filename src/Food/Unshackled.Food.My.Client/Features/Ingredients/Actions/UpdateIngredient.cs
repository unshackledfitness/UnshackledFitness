using MediatR;
using Unshackled.Food.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Ingredients.Actions;

public class UpdateIngredient
{
	public class Command : IRequest<CommandResult>
	{
		public FormIngredientModel Model { get; private set; }

		public Command(FormIngredientModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseIngredientHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}