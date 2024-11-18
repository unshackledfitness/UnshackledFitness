using MediatR;
using Unshackled.Food.My.Client.Features.Recipes.Models;

namespace Unshackled.Food.My.Client.Features.Recipes.Actions;

public class ListRecipeIngredientGroups
{
	public class Query : IRequest<List<RecipeIngredientGroupModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<RecipeIngredientGroupModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeIngredientGroupModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeIngredientGroupModel>>($"{baseUrl}get/{request.Sid}/groups") ??
				new List<RecipeIngredientGroupModel>();
		}
	}
}
