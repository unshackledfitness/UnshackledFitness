using MediatR;
using Unshackled.Fitness.My.Client.Features.CookbookRecipes.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Actions;

public class ListMemberHouseholds
{
	public class Query : IRequest<List<HouseholdListModel>> { }

	public class Handler : BaseCookbookRecipeHandler, IRequestHandler<Query, List<HouseholdListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<HouseholdListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<HouseholdListModel>>($"{baseUrl}list-member-households") ??
				new List<HouseholdListModel>();
		}
	}
}
