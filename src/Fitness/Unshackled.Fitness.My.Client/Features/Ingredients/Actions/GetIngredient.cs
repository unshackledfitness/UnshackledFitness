using MediatR;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Actions;

public class GetIngredient
{
	public class Query : IRequest<IngredientModel>
	{
		public string Key { get; private set; }

		public Query(string key)
		{
			Key = key;
		}
	}

	public class Handler : BaseIngredientHandler, IRequestHandler<Query, IngredientModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<IngredientModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<IngredientModel>($"{baseUrl}get/{request.Key}") ?? new();
		}
	}
}
