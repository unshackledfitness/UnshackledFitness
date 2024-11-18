using MediatR;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Actions;

public class UpdateCookbookProperties
{
	public class Command : IRequest<CommandResult<CookbookModel>>
	{
		public FormCookbookModel Model { get; private set; }

		public Command(FormCookbookModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult<CookbookModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<CookbookModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormCookbookModel, CookbookModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<CookbookModel>(false, Globals.UnexpectedError);
		}
	}
}
