﻿using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class UpdateStoreProperties
{
	public class Command : IRequest<CommandResult<StoreModel>>
	{
		public FormStoreModel Model { get; private set; }

		public Command(FormStoreModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult<StoreModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<StoreModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormStoreModel, StoreModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<StoreModel>(false, Globals.UnexpectedError);
		}
	}
}
