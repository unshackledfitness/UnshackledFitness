﻿using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class AddLocation
{
	public class Command : IRequest<CommandResult>
	{
		public FormStoreLocationModel Model { get; private set; }

		public Command(FormStoreLocationModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormStoreLocationModel, string>($"{baseUrl}add-location", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}