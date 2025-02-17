using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Stores.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores.Actions;

public class AddStore
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormStoreModel Model { get; private set; }

		public Command(FormStoreModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseStoreHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormStoreModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}