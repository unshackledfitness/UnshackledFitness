using MediatR;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.ActivityTemplates.Actions;

public class SearchTemplates
{
	public class Query : IRequest<SearchResult<TemplateListItem>>
	{
		public SearchTemplatesModel Model { get; private set; }

		public Query(SearchTemplatesModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseActivityTemplateHandler, IRequestHandler<Query, SearchResult<TemplateListItem>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<SearchResult<TemplateListItem>> Handle(Query request, CancellationToken cancellationToken)
		{
			return await PostToResultAsync<SearchTemplatesModel, SearchResult<TemplateListItem>>($"{baseUrl}search", request.Model) ??
				new SearchResult<TemplateListItem>();
		}
	}
}
