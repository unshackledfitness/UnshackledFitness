using MediatR;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public class ListMemberInvites
{
	public class Query : IRequest<List<InviteListModel>> 
	{
		public string CookbookSid { get; private set; }

		public Query(string cookbookSid)
		{
			CookbookSid = cookbookSid;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Query, List<InviteListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<InviteListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<InviteListModel>>($"{baseUrl}list/{request.CookbookSid}/invites") ??
				new List<InviteListModel>();
		}
	}
}
