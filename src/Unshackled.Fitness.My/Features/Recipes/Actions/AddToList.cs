using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

public class AddToList
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public AddRecipesToListModel Model { get; private set; }

		public Command(long memberId, long householdId, AddRecipesToListModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await db.AddRecipeItemsToList(request.MemberId, request.HouseholdId, request.Model);			
		}
	}
}