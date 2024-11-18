using MediatR;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Actions;

public class JoinCookbook
{
	public class Command : IRequest<CommandResult<CookbookListModel>>
	{
		public string CookbookSid { get; private set; }

		public Command(string groupSid)
		{
			CookbookSid = groupSid;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult<CookbookListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<CookbookListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<string, CookbookListModel>($"{baseUrl}join", request.CookbookSid)
				?? new CommandResult<CookbookListModel>(false, Globals.UnexpectedError);
		}
	}
}
