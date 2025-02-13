using MediatR;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class ListRecipeImages
{
	public class Query : IRequest<List<ImageModel>>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Query, List<ImageModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<ImageModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<ImageModel>>($"{baseUrl}get/{request.Sid}/images") ?? [];
		}
	}
}
