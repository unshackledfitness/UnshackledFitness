using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Features.Ingredients.Actions;

public class UpdateIngredient
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormIngredientModel Model { get; private set; }

		public Command(long memberId, long householdId, FormIngredientModel model)
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

			if (string.IsNullOrEmpty(request.Model.Key))
				return new CommandResult(false, "Invalid ingredient.");

			string newKey = request.Model.Title.NormalizeKey();

			var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				await db.RecipeIngredients
					.Where(x => x.HouseholdId == request.HouseholdId && x.Key == request.Model.Key)
					.UpdateFromQueryAsync(x => new RecipeIngredientEntity
					{
						Key = newKey,
						Title = request.Model.Title.Trim()
					}, cancellationToken);

				// Get old key product substitutions
				var subs = await db.ProductSubstitutions
					.AsNoTracking()
					.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == request.Model.Key)
					.OrderByDescending(x => x.IsPrimary)
					.ToListAsync(cancellationToken);

				if (subs.Count > 0)
				{
					// Check if new key has primary sub
					bool hasPrimary = await db.ProductSubstitutions
						.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == newKey && x.IsPrimary == true)
						.AnyAsync(cancellationToken);

					foreach (var sub in subs)
					{
						// Check if exists in new key
						bool exists = await db.ProductSubstitutions
							.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == newKey && x.ProductId == sub.ProductId)
							.AnyAsync(cancellationToken);

						if (exists)
						{
							// Delete
							await db.ProductSubstitutions
								.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == request.Model.Key && x.ProductId == sub.ProductId)
								.DeleteFromQueryAsync(cancellationToken);
						}
						else
						{
							// Move to new key
							await db.ProductSubstitutions
								.Where(x => x.HouseholdId == request.HouseholdId && x.IngredientKey == request.Model.Key && x.ProductId == sub.ProductId)
								.UpdateFromQueryAsync(x => new ProductSubstitutionEntity 
								{ 
									IngredientKey = newKey,
									IsPrimary = !hasPrimary
								},cancellationToken);

							hasPrimary = true;
						}
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Ingredient updated.");
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}