using MediatR;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;

public class AddToList
{
	public class Command : IRequest<CommandResult>
	{
		public AddRecipesToListModel Model { get; private set; }

		public Command(AddRecipesToListModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-to-shopping-list", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
