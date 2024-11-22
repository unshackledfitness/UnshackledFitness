using MediatR;
using Unshackled.Food.My.Client.Features.Recipes.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public class ListMemberCookbooks
{
	public class Query : IRequest<List<CookbookListModel>> { }

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<CookbookListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<CookbookListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<CookbookListModel>>($"{baseUrl}list-member-cookbooks") ??
				new List<CookbookListModel>();
		}
	}
}
