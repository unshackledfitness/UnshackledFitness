using MediatR;
using Unshackled.Food.My.Client.Features.Recipes.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public class ListMemberHouseholds
{
	public class Query : IRequest<List<HouseholdListModel>> { }

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<HouseholdListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<HouseholdListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<HouseholdListModel>>($"{baseUrl}list-member-households") ??
				new List<HouseholdListModel>();
		}
	}
}
