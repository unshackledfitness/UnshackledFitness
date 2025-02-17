using MediatR;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Actions;

public class GetProduct
{
	public class Query : IRequest<ProductModel>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseProductHandler, IRequestHandler<Query, ProductModel>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<ProductModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<ProductModel>($"{baseUrl}get/{request.Sid}") ?? new();
		}
	}
}
