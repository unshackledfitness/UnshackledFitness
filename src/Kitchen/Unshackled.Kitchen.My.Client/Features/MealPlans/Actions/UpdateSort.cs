﻿
using MediatR;
using Unshackled.Kitchen.My.Client.Features.MealPlans.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.MealPlans.Actions;

public class UpdateSort
{
	public class Command : IRequest<CommandResult>
	{
		public List<UpdateSortModel> Updates { get; private set; }

		public Command(List<UpdateSortModel> updates)
		{
			Updates = updates;
		}
	}

	public class Handler : BaseMealPlanHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-sort", request.Updates)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}