using MediatR;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Actions;

public class MakePrimary
{
	public class Command : IRequest<CommandResult>
	{
		public FormSubstitutionModel Model { get; private set; }

		public Command(FormSubstitutionModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseIngredientHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}make-primary", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}