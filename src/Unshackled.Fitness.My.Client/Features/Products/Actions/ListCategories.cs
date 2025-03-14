﻿using MediatR;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products.Actions;

public class ListCategories
{
	public class Query : IRequest<List<CategoryModel>> { }

	public class Handler : BaseProductHandler, IRequestHandler<Query, List<CategoryModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<List<CategoryModel>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<List<CategoryModel>>($"{baseUrl}list-categories")
				?? new();
		}
	}
}
