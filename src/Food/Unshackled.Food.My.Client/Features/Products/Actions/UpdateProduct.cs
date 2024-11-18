using MediatR;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Products.Actions;

public class UpdateProduct
{
	public class Command : IRequest<CommandResult<ProductModel>>
	{
		public FormProductModel Model { get; private set; }

		public Command(FormProductModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Command, CommandResult<ProductModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ProductModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormProductModel, ProductModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<ProductModel>(false, Globals.UnexpectedError);
		}
	}
}