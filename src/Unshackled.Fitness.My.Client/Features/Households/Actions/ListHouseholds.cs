using MediatR;
using Unshackled.Fitness.My.Client.Features.Households.Models;

namespace Unshackled.Fitness.My.Client.Features.Households.Actions;

public class ListHouseholds
{
	public class Query : IRequest<List<HouseholdListModel>> { }

	public class Handler : BaseHouseholdHandler, IRequestHandler<Query, List<HouseholdListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<HouseholdListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<HouseholdListModel>>($"{baseUrl}list") ??
				new List<HouseholdListModel>();
		}
	}
}
