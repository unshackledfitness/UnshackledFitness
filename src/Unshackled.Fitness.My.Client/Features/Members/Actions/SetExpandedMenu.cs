using MediatR;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Members.Actions;

public class SetExpandedMenu
{
	public class Command : IRequest<Unit>
	{
		public string MenuId { get; private set; }

		public Command(string menuId)
		{
			MenuId = menuId;
		}
	}

	public class Handler : BaseMemberHandler, IRequestHandler<Command, Unit>
	{
		private readonly AppState state = default!;
		private readonly ILocalStorage localStorage = default!;

		public Handler(HttpClient httpClient, ILocalStorage localStorage, AppState state) : base(httpClient)
		{
			this.state = state;
			this.localStorage = localStorage;
		}

		public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
		{
			if (state.ExpandedMenuId == request.MenuId)
				return Unit.Value;

			if (string.IsNullOrEmpty(request.MenuId))
			{
				await localStorage.RemoveItemAsync(AppGlobals.LocalStorageKeys.ExpandedMenuId);
			}
			await localStorage.SetItemAsStringAsync(AppGlobals.LocalStorageKeys.ExpandedMenuId, request.MenuId);
			state.SetExpandedMenu(request.MenuId);

			return Unit.Value;
		}

	}
}
