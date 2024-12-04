using MediatR;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Services;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

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
			var list = await localStorage.GetItemAsync<IngredientTitleList>(KitchenGlobals.LocalStorageKeys.IngredientTitles);

			if (list == null || list.DateRetrieved <= DateTime.UtcNow.AddMinutes(-Globals.DefaultCacheDurationMinutes))
			{
				list = new() { DateRetrieved = DateTime.UtcNow };
				list.Titles = await GetResultAsync<List<string>>($"{baseUrl}list-ingredient-titles") ?? new();

				await localStorage.SetItemAsync(KitchenGlobals.LocalStorageKeys.IngredientTitles, list, cancellationToken);
			}

			return list.Titles
				.Where(x => x.StartsWith(request.Value))
				.ToList();
		}
	}
}
