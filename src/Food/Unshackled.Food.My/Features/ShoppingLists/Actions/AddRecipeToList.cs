using AutoMapper;
using MediatR;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Models.ShoppingLists;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Features.ShoppingLists.Actions;

public class AddRecipeToList
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public AddRecipeToListModel Model { get; private set; }

		public Command(long memberId, long householdId, AddRecipeToListModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await db.AddRecipeItemsToList(request.MemberId, request.HouseholdId, request.Model);
		}
	}
}