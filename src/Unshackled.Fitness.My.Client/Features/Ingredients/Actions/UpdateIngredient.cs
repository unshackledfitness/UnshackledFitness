﻿using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Actions;

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