using MediatR;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public class GetCookbook
{
	public class Query : IRequest<CookbookModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Query, CookbookModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CookbookModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<CookbookModel>($"{baseUrl}get/{request.Sid}") ??
				new CookbookModel();
		}
	}
}
