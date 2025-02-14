using MediatR;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Actions;

public class ListMakeIt
{
	public class Query : IRequest<List<MakeItRecipeModel>> 
	{
		public Dictionary<string, decimal> RecipesAndScales { get; private set; }

		public Query(Dictionary<string, decimal> recipesAndScales)
		{
			RecipesAndScales = recipesAndScales;
		}
	}

	public class Handler : BaseDashboardHandler, IRequestHandler<Query, List<MakeItRecipeModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<MakeItRecipeModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<Dictionary<string, decimal>, List<MakeItRecipeModel>>($"{baseUrl}list-make-it", request.RecipesAndScales) ?? [];
		}
	}
}
