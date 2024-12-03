using MediatR;

namespace Unshackled.Food.My.Client.Features.Households.Actions;

public class CanDeleteHousehold
{
	public class Query : IRequest<bool>
	{
		public string Sid { get; private set; }

		public Query(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseHouseholdHandler, IRequestHandler<Query, bool>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
		{
			return await GetResultAsync<bool>($"{baseUrl}can-delete/{request.Sid}") ;
		}
	}
}
