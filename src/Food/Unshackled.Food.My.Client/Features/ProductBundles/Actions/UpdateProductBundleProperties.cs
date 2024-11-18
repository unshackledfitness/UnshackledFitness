using MediatR;
using Unshackled.Food.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.ProductBundles.Actions;

public class UpdateProductBundleProperties
{
	public class Command : IRequest<CommandResult<ProductBundleModel>>
	{
		public FormProductBundleModel Model { get; private set; }

		public Command(FormProductBundleModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Command, CommandResult<ProductBundleModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ProductBundleModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormProductBundleModel, ProductBundleModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<ProductBundleModel>(false, Globals.UnexpectedError);
		}
	}
}
