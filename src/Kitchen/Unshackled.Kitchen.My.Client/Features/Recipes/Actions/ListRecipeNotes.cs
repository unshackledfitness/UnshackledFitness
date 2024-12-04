using MediatR;
using Unshackled.Kitchen.My.Client.Features.Recipes.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes.Actions;

public class ListRecipeNotes
{
	public class Query : IRequest<List<RecipeNoteModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<RecipeNoteModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<RecipeNoteModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<RecipeNoteModel>>($"{baseUrl}get/{request.Sid}/notes") ??
				new List<RecipeNoteModel>();
		}
	}
}
