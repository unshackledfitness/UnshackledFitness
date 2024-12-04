using MediatR;
using Unshackled.Kitchen.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.ProductBundles.Actions;

public class DeleteProduct
{
	public class Command : IRequest<CommandResult>
	{
		public DeleteProductModel Model { get; private set; }

		public Command(DeleteProductModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete-product", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
