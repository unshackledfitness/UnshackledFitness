using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public class AddInvite
{
	public class Command : IRequest<CommandResult<InviteListModel>>
	{
		public string CookbookSid { get; private set; }
		public FormAddInviteModel Model { get; private set; }

		public Command(string cookbookSid, FormAddInviteModel model)
		{
			CookbookSid = cookbookSid;
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
