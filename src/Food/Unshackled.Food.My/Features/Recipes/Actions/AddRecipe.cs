using AutoMapper;
using MediatR;
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

public class AddRecipe
{
	public class Command : IRequest<CommandResult<string>>
	{
		public long MemberId { get; private set; }
		public long HouseholdId { get; private set; }
		public FormRecipeModel Model { get; private set; }

		public Command(long memberId, long householdId, FormRecipeModel model)
		{
			MemberId = memberId;
			HouseholdId = householdId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			if(!await db.HasHouseholdPermission(request.HouseholdId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<string>(false, FoodGlobals.PermissionError);

			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				RecipeEntity recipe = new()
				{
					CookTimeMinutes = request.Model.CookTimeMinutes,
					Description = request.Model.Description?.Trim(),
					HouseholdId = request.HouseholdId,
					IsAustralianCuisine = request.Model.IsAustralianCuisine,
					IsCajunCreoleCuisine = request.Model.IsCajunCreoleCuisine,
					IsCaribbeanCuisine = request.Model.IsCaribbeanCuisine,
					IsCentralAfricanCuisine = request.Model.IsCentralAfricanCuisine,
					IsCentralAmericanCuisine = request.Model.IsCentralAmericanCuisine,
					IsCentralAsianCuisine = request.Model.IsCentralAsianCuisine,
					IsCentralEuropeanCuisine = request.Model.IsCentralEuropeanCuisine,
					IsChineseCuisine = request.Model.IsChineseCuisine,
					IsEastAfricanCuisine = request.Model.IsEastAfricanCuisine,
					IsEastAsianCuisine = request.Model.IsEastAsianCuisine,
					IsEasternEuropeanCuisine = request.Model.IsEasternEuropeanCuisine,
					IsFilipinoCuisine = request.Model.IsFilipinoCuisine,
					IsFrenchCuisine = request.Model.IsFrenchCuisine,
					IsFusionCuisine = request.Model.IsFusionCuisine,
					IsGermanCuisine = request.Model.IsGermanCuisine,
					IsGlutenFree = request.Model.IsGlutenFree,
					IsGreekCuisine = request.Model.IsGreekCuisine,
					IsIndianCuisine = request.Model.IsIndianCuisine,
					IsIndonesianCuisine = request.Model.IsIndonesianCuisine,
					IsItalianCuisine = request.Model.IsItalianCuisine,
					IsJapaneseCuisine = request.Model.IsJapaneseCuisine,
					IsKoreanCuisine = request.Model.IsKoreanCuisine,
					IsMexicanCuisine = request.Model.IsMexicanCuisine,
					IsNativeAmericanCuisine = request.Model.IsNativeAmericanCuisine,
					IsNorthAfricanCuisine = request.Model.IsNorthAfricanCuisine,
					IsNorthAmericanCuisine = request.Model.IsNorthAmericanCuisine,
					IsNorthernEuropeanCuisine = request.Model.IsNorthernEuropeanCuisine,
					IsNutFree = request.Model.IsNutFree,
					IsOceanicCuisine = request.Model.IsOceanicCuisine,
					IsPakistaniCuisine = request.Model.IsPakistaniCuisine,
					IsPolishCuisine = request.Model.IsPolishCuisine,
					IsPolynesianCuisine = request.Model.IsPolynesianCuisine,
					IsRussianCuisine = request.Model.IsRussianCuisine,
					IsSoulFoodCuisine = request.Model.IsSoulFoodCuisine,
					IsSouthAfricanCuisine = request.Model.IsSouthAfricanCuisine,
					IsSouthAmericanCuisine = request.Model.IsSouthAmericanCuisine,
					IsSouthAsianCuisine = request.Model.IsSouthAsianCuisine,
					IsSoutheastAsianCuisine = request.Model.IsSoutheastAsianCuisine,
					IsSouthernEuropeanCuisine = request.Model.IsSouthernEuropeanCuisine,
					IsSpanishCuisine = request.Model.IsSpanishCuisine,
					IsTexMexCuisine = request.Model.IsTexMexCuisine,
					IsThaiCuisine = request.Model.IsThaiCuisine,
					IsVegan = request.Model.IsVegan,
					IsVegetarian = request.Model.IsVegetarian,
					IsVietnameseCuisine = request.Model.IsVietnameseCuisine,
					IsWestAfricanCuisine = request.Model.IsWestAfricanCuisine,
					IsWestAsianCuisine = request.Model.IsWestAsianCuisine,
					IsWesternEuropeanCuisine = request.Model.IsWesternEuropeanCuisine,
					PrepTimeMinutes = request.Model.PrepTimeMinutes,
					Title = request.Model.Title.Trim(),
					TotalServings = request.Model.TotalServings
				};
				db.Recipes.Add(recipe);
				await db.SaveChangesAsync(cancellationToken);

				RecipeIngredientGroupEntity group = new()
				{
					HouseholdId = request.HouseholdId,
					RecipeId = recipe.Id,
					SortOrder = 0,
					Title = string.Empty
				};
				db.RecipeIngredientGroups.Add(group);
				await db.SaveChangesAsync(cancellationToken);

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<string>(true, "Recipe created.", recipe.Id.Encode());
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<string>(false, Globals.UnexpectedError);
			}
		}
	}
}