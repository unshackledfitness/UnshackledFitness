using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Products.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Actions;

public class MergeProducts
{
	public class Command : IRequest<CommandResult>
	{
		public MergeModel Model { get; }

		public Command(MergeModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}merge", request.Model) ??
				new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
