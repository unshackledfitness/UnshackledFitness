using MediatR;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class ListRecipeTags
{
	public class Query : IRequest<List<RecipeTagSelectItem>> { }

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<RecipeTagSelectItem>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeTagSelectItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeTagSelectItem>>($"{baseUrl}list-recipe-tags") ??
				[];
		}
	}
}
