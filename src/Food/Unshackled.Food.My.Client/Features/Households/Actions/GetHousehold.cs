using MediatR;
using Unshackled.Food.My.Client.Features.Households.Models;

namespace Unshackled.Food.My.Client.Features.Households.Actions;

public class GetHousehold
{
	public class Query : IRequest<HouseholdModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Query, HouseholdModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<HouseholdModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<HouseholdModel>($"{baseUrl}get/{request.Sid}") ??
				new HouseholdModel();
		}
	}
}
