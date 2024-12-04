using MediatR;
using Unshackled.Kitchen.My.Client.Features.Households.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Households.Actions;

public class SearchMembers
{
	public class Query : IRequest<SearchResult<MemberListModel>> 
	{
		public string HouseholdSid { get; private set; }
		public MemberSearchModel Model { get; private set; }

		public Query(string householdSid, MemberSearchModel model)
		{
			HouseholdSid = householdSid;
			Model = model;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Query, SearchResult<MemberListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<MemberListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<MemberSearchModel, SearchResult<MemberListModel>>($"{baseUrl}search/{request.HouseholdSid}/members", request.Model) ??
				new SearchResult<MemberListModel>();
		}
	}
}
