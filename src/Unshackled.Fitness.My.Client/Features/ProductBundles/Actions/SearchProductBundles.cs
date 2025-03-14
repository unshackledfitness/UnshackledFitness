﻿using MediatR;
using Unshackled.Fitness.My.Client.Features.ProductBundles.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ProductBundles.Actions;

public class SearchProductBundles
{
	public class Query : IRequest<SearchResult<ProductBundleListModel>>
	{
		public SearchProductBundlesModel Model { get; private set; }

		public Query(SearchProductBundlesModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseProductBundleHandler, IRequestHandler<Query, SearchResult<ProductBundleListModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<ProductBundleListModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchProductBundlesModel, SearchResult<ProductBundleListModel>>($"{baseUrl}search", request.Model) ??
				new SearchResult<ProductBundleListModel>();
		}
	}
}
