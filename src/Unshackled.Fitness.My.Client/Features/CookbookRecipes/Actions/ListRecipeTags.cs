﻿using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.CookbookRecipes.Actions;

public class ListRecipeTags
{
	public class Query : IRequest<List<RecipeTagSelectItem>> { }

	public class Handler : BaseCookbookRecipeHandler, IRequestHandler<Query, List<RecipeTagSelectItem>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeTagSelectItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeTagSelectItem>>($"{baseUrl}list-recipe-tags") ??
				[];
		}
	}
}
