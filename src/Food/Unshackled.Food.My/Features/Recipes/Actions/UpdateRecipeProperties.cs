using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Recipes.Actions;

public class UpdateRecipeProperties
{
	public class Command : IRequest<CommandResult<RecipeModel>>
	{
		public long MemberId { get; private set; }
		public FormRecipeModel Model { get; private set; }

		public Command(long memberId, FormRecipeModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<RecipeModel>>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<RecipeModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			long recipeId = request.Model.Sid.DecodeLong();

			if (recipeId == 0)
				return new CommandResult<RecipeModel>(false, "Invalid recipe ID.");

			if (!await db.HasRecipePermission(recipeId, request.MemberId, PermissionLevels.Write))
				return new CommandResult<RecipeModel>(false, FoodGlobals.PermissionError);

			RecipeEntity? recipe = await db.Recipes
				.Where(x => x.Id == recipeId)
				.SingleOrDefaultAsync();

			if (recipe == null)
				return new CommandResult<RecipeModel>(false, "Invalid recipe.");

			// Update recipe
			recipe.CookTimeMinutes = request.Model.CookTimeMinutes;
			recipe.Description = request.Model.Description?.Trim();
			recipe.IsAustralianCuisine = request.Model.IsAustralianCuisine;
			recipe.IsCajunCreoleCuisine = request.Model.IsCajunCreoleCuisine;
			recipe.IsCaribbeanCuisine = request.Model.IsCaribbeanCuisine;
			recipe.IsCentralAfricanCuisine = request.Model.IsCentralAfricanCuisine;
			recipe.IsCentralAmericanCuisine = request.Model.IsCentralAmericanCuisine;
			recipe.IsCentralAsianCuisine = request.Model.IsCentralAsianCuisine;
			recipe.IsCentralEuropeanCuisine = request.Model.IsCentralEuropeanCuisine;
			recipe.IsChineseCuisine = request.Model.IsChineseCuisine;
			recipe.IsEastAfricanCuisine = request.Model.IsEastAfricanCuisine;
			recipe.IsEastAsianCuisine = request.Model.IsEastAsianCuisine;
			recipe.IsEasternEuropeanCuisine = request.Model.IsEasternEuropeanCuisine;
			recipe.IsFilipinoCuisine = request.Model.IsFilipinoCuisine;
			recipe.IsFrenchCuisine = request.Model.IsFrenchCuisine;
			recipe.IsFusionCuisine = request.Model.IsFusionCuisine;
			recipe.IsGermanCuisine = request.Model.IsGermanCuisine;
			recipe.IsGlutenFree = request.Model.IsGlutenFree;
			recipe.IsGreekCuisine = request.Model.IsGreekCuisine;
			recipe.IsIndianCuisine = request.Model.IsIndianCuisine;
			recipe.IsIndonesianCuisine = request.Model.IsIndonesianCuisine;
			recipe.IsItalianCuisine = request.Model.IsItalianCuisine;
			recipe.IsJapaneseCuisine = request.Model.IsJapaneseCuisine;
			recipe.IsKoreanCuisine = request.Model.IsKoreanCuisine;
			recipe.IsMexicanCuisine = request.Model.IsMexicanCuisine;
			recipe.IsNativeAmericanCuisine = request.Model.IsNativeAmericanCuisine;
			recipe.IsNorthAfricanCuisine = request.Model.IsNorthAfricanCuisine;
			recipe.IsNorthAmericanCuisine = request.Model.IsNorthAmericanCuisine;
			recipe.IsNorthernEuropeanCuisine = request.Model.IsNorthernEuropeanCuisine;
			recipe.IsNutFree = request.Model.IsNutFree;
			recipe.IsOceanicCuisine = request.Model.IsOceanicCuisine;
			recipe.IsPakistaniCuisine = request.Model.IsPakistaniCuisine;
			recipe.IsPolishCuisine = request.Model.IsPolishCuisine;
			recipe.IsPolynesianCuisine = request.Model.IsPolynesianCuisine;
			recipe.IsRussianCuisine = request.Model.IsRussianCuisine;
			recipe.IsSoulFoodCuisine = request.Model.IsSoulFoodCuisine;
			recipe.IsSouthAfricanCuisine = request.Model.IsSouthAfricanCuisine;
			recipe.IsSouthAmericanCuisine = request.Model.IsSouthAmericanCuisine;
			recipe.IsSouthAsianCuisine = request.Model.IsSouthAsianCuisine;
			recipe.IsSoutheastAsianCuisine = request.Model.IsSoutheastAsianCuisine;
			recipe.IsSouthernEuropeanCuisine = request.Model.IsSouthernEuropeanCuisine;
			recipe.IsSpanishCuisine = request.Model.IsSpanishCuisine;
			recipe.IsTexMexCuisine = request.Model.IsTexMexCuisine;
			recipe.IsThaiCuisine = request.Model.IsThaiCuisine;
			recipe.IsVegan = request.Model.IsVegan;
			recipe.IsVegetarian = request.Model.IsVegetarian;
			recipe.IsVietnameseCuisine = request.Model.IsVietnameseCuisine;
			recipe.IsWestAfricanCuisine = request.Model.IsWestAfricanCuisine;
			recipe.IsWestAsianCuisine = request.Model.IsWestAsianCuisine;
			recipe.IsWesternEuropeanCuisine = request.Model.IsWesternEuropeanCuisine;
			recipe.PrepTimeMinutes = request.Model.PrepTimeMinutes;
			recipe.Title = request.Model.Title.Trim();
			recipe.TotalServings = request.Model.TotalServings;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult<RecipeModel>(true, "Recipe updated.", mapper.Map<RecipeModel>(recipe));
		}
	}
}