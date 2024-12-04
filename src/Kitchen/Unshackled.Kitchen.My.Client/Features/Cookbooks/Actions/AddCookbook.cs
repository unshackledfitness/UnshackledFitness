using MediatR;
using Unshackled.Kitchen.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Cookbooks.Actions;

public class AddCookbook
{
	public class Command : IRequest<CommandResult<CookbookListModel>>
	{
		public FormCookbookModel Model { get; private set; }

		public Command(FormCookbookModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult<CookbookListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<CookbookListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormCookbookModel, CookbookListModel>($"{baseUrl}add", request.Model)
				?? new CommandResult<CookbookListModel>(false, Globals.UnexpectedError);
		}
	}
}
