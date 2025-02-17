using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Client.Services;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class SearchIngredients
{
	public class Query : IRequest<List<string>>
	{
		public string Value { get; private set; }

		public Query(string value)
		{
			Value = value;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<string>>
	{
		private readonly ILocalStorage localStorage;
		public Handler(HttpClient httpClient, ILocalStorage storage) : base(httpClient) 
		{
			this.localStorage = storage;
		}

		public async Task<List<string>> Handle(Query request, CancellationToken cancellationToken)
		{
			var list = await localStorage.GetItemAsync<IngredientTitleList>(Globals.LocalStorageKeys.IngredientTitles);

			if (list == null || list.DateRetrieved <= DateTime.UtcNow.AddMinutes(-Globals.DefaultCacheDurationMinutes))
			{
				list = new() { DateRetrieved = DateTime.UtcNow };
				list.Titles = await GetResultAsync<List<string>>($"{baseUrl}list-ingredient-titles") ?? new();

				await localStorage.SetItemAsync(Globals.LocalStorageKeys.IngredientTitles, list, cancellationToken);
			}

			return list.Titles
				.Where(x => x.StartsWith(request.Value))
				.ToList();
		}
	}
}
