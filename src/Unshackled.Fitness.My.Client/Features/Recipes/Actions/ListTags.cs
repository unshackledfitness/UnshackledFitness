using MediatR;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class ListTags
{
	public class Query : IRequest<List<TagModel>> { }

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<TagModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<TagModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<TagModel>>($"{baseUrl}list-tags") ?? [];
		}
	}
}
