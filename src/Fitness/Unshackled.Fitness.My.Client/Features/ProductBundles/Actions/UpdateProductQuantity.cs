using MediatR;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;

public class UpdateProductQuantity
{
	public class Command : IRequest<CommandResult>
	{
		public UpdateProductModel Model { get; private set; }

		public Command(UpdateProductModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}update-product", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
