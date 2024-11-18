using MediatR;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Actions;

public class AddInvite
{
	public class Command : IRequest<CommandResult<InviteListModel>>
	{
		public string CookbookSid { get; private set; }
		public FormAddInviteModel Model { get; private set; }

		public Command(string groupSid, FormAddInviteModel model)
		{
			CookbookSid = groupSid;
			Model = model;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult<InviteListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<InviteListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormAddInviteModel, InviteListModel>($"{baseUrl}invite/{request.CookbookSid}", request.Model)
				?? new CommandResult<InviteListModel>(false, Globals.UnexpectedError);
		}
	}
}
