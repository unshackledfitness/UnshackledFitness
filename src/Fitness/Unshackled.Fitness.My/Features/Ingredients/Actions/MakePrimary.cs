using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Ingredients.Actions;

public class MakePrimary
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormSubstitutionModel Model { get; private set; }

		public Command(long memberId, long householdId, FormSubstitutionModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper map) : base(db, map) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			if (!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			long productId = request.Model.ProductSid.DecodeLong();

			if (productId == 0)
				return new CommandResult(false, "Invalid product ID.");

			var sub = await db.ProductSubstitutions
				.Where(x => x.IngredientKey == request.Model.IngredientKey && x.ProductId == productId)
				.SingleOrDefaultAsync();

			if (sub == null)
				return new CommandResult(false, "Invalid product substitution.");

			// Clear existing
			await db.ProductSubstitutions
				.Where(x => x.IngredientKey == request.Model.IngredientKey && x.HouseholdId == request.HouseholdId && x.IsPrimary == true)
				.UpdateFromQueryAsync(x => new ProductSubstitutionEntity { IsPrimary = false });

			sub.IsPrimary = true;
			await db.SaveChangesAsync();

			return new CommandResult(true, "Product substitution set as primary.");
		}
	}
}