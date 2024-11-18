using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Recipes.Actions;

public class UpdateNotes
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public long RecipeId { get; private set; }
		public UpdateNotesModel Model { get; private set; }

		public Command(long memberId, long recipeId, UpdateNotesModel model)
		{
			MemberId = memberId;
			RecipeId = recipeId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.RecipeId == 0)
				return new CommandResult(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, FoodGlobals.PermissionError);

			var recipe = await db.Recipes
				.Where(x => x.Id == request.RecipeId)
				.SingleOrDefaultAsync();

			if(recipe == null)
				return new CommandResult(false, "Invalid recipe.");

			var currentNotes = await db.RecipeNotes
				.Where(x => x.RecipeId == request.RecipeId)
				.OrderBy(x => x.SortOrder)
				.ToListAsync();

			using var transaction = await db.Database.BeginTransactionAsync();

			try
			{
				// Delete Notes
				foreach (var note in request.Model.DeletedNotes)
				{
					// Find existing
					var existing = currentNotes
						.Where(x => x.Id == note.Sid.DecodeLong())
						.SingleOrDefault();

					if (existing != null)
					{
						db.RecipeNotes.Remove(existing);
					}
					await db.SaveChangesAsync(cancellationToken);
				}

				foreach (var item in request.Model.Notes)
				{
					RecipeNoteEntity? note = null;

					// Add new
					if(item.IsNew)
					{
						note = new RecipeNoteEntity
						{
							HouseholdId = recipe.HouseholdId,
							Note = item.Note.Trim(),
							RecipeId = request.RecipeId,
							SortOrder = item.SortOrder
						};
						db.RecipeNotes.Add(note);
						await db.SaveChangesAsync();
					}
					else // Update
					{
						note = currentNotes
							.Where(x => x.Id == item.Sid.DecodeLong())
							.SingleOrDefault();

						if(note != null) 
						{
							note.Note = item.Note.Trim();
							note.SortOrder = item.SortOrder;
							await db.SaveChangesAsync();
						}
					}
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult(true, "Notes updated.");
			}
			catch
			{
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}