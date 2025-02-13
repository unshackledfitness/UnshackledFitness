using MediatR;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;

public class AddProductBundle
{
	public class Command : IRequest<CommandResult<string>>
	{
		public FormProductBundleModel Model { get; private set; }

		public Command(FormProductBundleModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormProductBundleModel, string>($"{baseUrl}add", request.Model)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}