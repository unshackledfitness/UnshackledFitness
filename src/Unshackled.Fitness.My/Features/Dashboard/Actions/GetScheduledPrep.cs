using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Dashboard.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Dashboard.Actions;

public class GetScheduledPrep
{
	public class Query : IRequest<ScheduledPrepModel>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public DateOnly DisplayDate { get; private set; }

		public Query(long memberId, long householdId, DateOnly displayDate)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			DisplayDate = displayDate;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ScheduledPrepModel>
	{
		public Handler(BaseDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<ScheduledPrepModel> Handle(Query request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Read))
				return new();

			var model = new ScheduledPrepModel
			{
				Slots = await db.MealPrepPlanSlots
					.Where(x => x.HouseholdId == request.HouseholdId)
					.OrderBy(x => x.SortOrder)
					.Select(x => new SlotModel
					{
						DateCreatedUtc = x.DateCreatedUtc,
						DateLastModifiedUtc = x.DateLastModifiedUtc,
						HouseholdSid = x.HouseholdId.Encode(),
						Sid = x.Id.Encode(),
						SortOrder = x.SortOrder,
						Title = x.Title
					})
					.ToListAsync(cancellationToken),

				Recipes = await db.MealPrepPlanRecipes
					.Include(x => x.Recipe)
					.Where(x => x.HouseholdId == request.HouseholdId && x.DatePlanned == request.DisplayDate)
					.OrderBy(x => x.SlotId)
						.ThenBy(x => x.Recipe.Title)
					.Select(x => new MealPrepPlanRecipeModel
					{
						DateCreatedUtc = x.DateCreatedUtc,
						DateLastModifiedUtc = x.DateLastModifiedUtc,
						HouseholdSid = x.HouseholdId.Encode(),
						ListGroupSid = x.SlotId.HasValue ? x.SlotId.Value.Encode() : string.Empty,
						RecipeSid = x.RecipeId.Encode(),
						RecipeTitle = x.Recipe.Title,
						Scale = x.Scale,
						Sid = x.Id.Encode()
					})
					.ToListAsync(cancellationToken)
			};

			if (model.Slots.Count != 0)
			{
				int sortOrder = 0;
				foreach (var slot in model.Slots)
				{
					foreach (var recipe in model.Recipes.Where(x => x.ListGroupSid == slot.Sid).ToList())
					{
						recipe.SortOrder = sortOrder;
						sortOrder++;
					}
				}

				model.Recipes = model.Recipes.OrderBy(x => x.SortOrder).ToList();
			}
			else
			{
				model.Slots.Add(new SlotModel
				{
					Sid = Globals.UncategorizedKey,
					Title = string.Empty
				});

				model.Recipes.ForEach(x => x.ListGroupSid = Globals.UncategorizedKey);
			}

			return model;
		}
	}
}
