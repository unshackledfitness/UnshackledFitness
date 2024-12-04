using MediatR;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

public class ListRecipeSteps
{
	public class Query : IRequest<List<RecipeStepModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<RecipeStepModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeStepModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeStepModel>>($"{baseUrl}get/{request.Sid}/steps") ??
				new List<RecipeStepModel>();
		}
	}
}
