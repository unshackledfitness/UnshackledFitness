using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Products.Actions;

public class UploadImage
{
	public class Command : IRequest<CommandResult<ImageModel>>
	{
		public string ProductSid { get; private set; }
		public MultipartFormDataContent Content { get; private set; }

		public Command(string productSid, MultipartFormDataContent content)
		{
			ProductSid = productSid;
			Content = content;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Command, CommandResult<ImageModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ImageModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostMultipartFormDataToCommandResultAsync<ImageModel>($"{baseUrl}upload-image/{request.ProductSid}", request.Content)
				?? new CommandResult<ImageModel>(false, Globals.UnexpectedError);
		}
	}
}
