using MediatR;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

public class ListRecipeIngredients
{
	public class Query : IRequest<List<RecipeIngredientModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<RecipeIngredientModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeIngredientModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeIngredientModel>>($"{baseUrl}get/{request.Sid}/ingredients") ??
				new List<RecipeIngredientModel>();
		}
	}
}
