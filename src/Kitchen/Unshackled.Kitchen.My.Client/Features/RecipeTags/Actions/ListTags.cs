using MediatR;
using Unshackled.Kitchen.My.Client.Features.RecipeTags.Models;

namespace Unshackled.Kitchen.My.Client.Features.RecipeTags.Actions;

public class ListTags
{
	public class Query : IRequest<List<TagModel>> { }

	public class Handler : BaseRecipeTagHandler, IRequestHandler<Query, List<TagModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<TagModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<TagModel>>($"{baseUrl}list")
				?? new();
		}
	}
}
