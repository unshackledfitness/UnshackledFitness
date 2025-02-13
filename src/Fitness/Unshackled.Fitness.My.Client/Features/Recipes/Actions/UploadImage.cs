using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class UploadImage
{
	public class Command : IRequest<CommandResult<ImageModel>>
	{
		public string RecipeSid { get; private set; }
		public MultipartFormDataContent Content { get; private set; }

		public Command(string recipeSid, MultipartFormDataContent content)
		{
			RecipeSid = recipeSid;
			Content = content;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult<ImageModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ImageModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostMultipartFormDataToCommandResultAsync<ImageModel>($"{baseUrl}upload-image/{request.RecipeSid}", request.Content)
				?? new CommandResult<ImageModel>(false, Globals.UnexpectedError);
		}
	}
}
