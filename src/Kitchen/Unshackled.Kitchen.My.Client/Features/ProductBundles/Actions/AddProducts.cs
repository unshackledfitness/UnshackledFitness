using MediatR;
using Unshackled.Kitchen.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.ProductBundles.Actions;

public class AddProducts
{
	public class Command : IRequest<CommandResult>
	{
		public AddProductsModel Model { get; private set; }

		public Command(AddProductsModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add-products", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}