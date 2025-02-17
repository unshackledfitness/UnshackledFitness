using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public class MakeOwner
{
	public class Command : IRequest<CommandResult>
	{
		public MakeOwnerModel Model { get; private set; }

		public Command(MakeOwnerModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}make-owner", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
