using MediatR;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;

namespace Unshackled.Food.My.Client.Features.Cookbooks.Actions;

public class ListCookbooks
{
	public class Query : IRequest<List<CookbookListModel>> { }

	public class Handler : BaseCookbookHandler, IRequestHandler<Query, List<CookbookListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<CookbookListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<CookbookListModel>>($"{baseUrl}list") ??
				new List<CookbookListModel>();
		}
	}
}
