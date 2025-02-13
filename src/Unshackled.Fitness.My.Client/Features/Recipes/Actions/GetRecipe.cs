using MediatR;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class GetRecipe
{
	public class Query : IRequest<RecipeModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, RecipeModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<RecipeModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<RecipeModel>($"{baseUrl}get/{request.Sid}") ??
				new RecipeModel();
		}
	}
}
