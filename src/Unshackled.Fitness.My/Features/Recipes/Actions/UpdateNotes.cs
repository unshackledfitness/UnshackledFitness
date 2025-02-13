using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Recipes.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Recipes.Actions;

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
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{			
			if (request.RecipeId == 0)
				return new CommandResult(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(request.RecipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult(false, Globals.PermissionError);

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
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult(false, Globals.UnexpectedError);
			}
		}
	}
}