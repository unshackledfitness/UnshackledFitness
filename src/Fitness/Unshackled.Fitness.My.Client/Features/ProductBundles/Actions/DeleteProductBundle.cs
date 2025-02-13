using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;

public class DeleteProductBundle
{
	public class Command : IRequest<CommandResult>
	{
		public string ProductBundleSid { get; private set; }

		public Command(string productBundleSid)
		{
			ProductBundleSid = productBundleSid;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}delete", request.ProductBundleSid)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}
