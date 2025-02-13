using MediatR;
using Unshackled.Fitness.My.Client.Features.Households.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public class ListMemberInvites
{
	public class Query : IRequest<List<InviteListModel>> 
	{
		public string HouseholdSid { get; private set; }

		public Query(string householdSid)
		{
			HouseholdSid = householdSid;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Query, List<InviteListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<InviteListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<InviteListModel>>($"{baseUrl}list/{request.HouseholdSid}/invites") ??
				new List<InviteListModel>();
		}
	}
}
