using MediatR;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Members.Actions;

public class GetExpandedMenu
{
	public class Query : IRequest<Unit> { }

	public class Handler : BaseMemberHandler, IRequestHandler<Query, Unit>
	{
		private readonly AppState state = default!;
		private readonly ILocalStorage localStorage = default!;

		public Handler(HttpClient httpClient, ILocalStorage localStorage, AppState state) : base(httpClient)
		{
			this.state = state;
			this.localStorage = localStorage;
		}

		public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
		{
			string menuId = await localStorage.GetItemAsStringAsync(AppGlobals.LocalStorageKeys.ExpandedMenuId) ?? string.Empty;
			state.SetExpandedMenu(menuId);
			return Unit.Value;
		}

	}
}
