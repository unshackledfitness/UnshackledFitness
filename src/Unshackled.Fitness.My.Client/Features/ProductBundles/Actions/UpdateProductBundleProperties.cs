using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;

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
