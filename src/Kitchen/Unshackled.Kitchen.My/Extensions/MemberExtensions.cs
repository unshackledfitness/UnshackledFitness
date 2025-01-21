using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data.Entities;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Extensions;

public static class MemberExtensions
{
	public static async Task<MemberEntity?> AddMember(this KitchenDbContext db, string email, SiteConfiguration siteConfig)
	{
		email = email.Trim().ToLower();

		if (string.IsNullOrEmpty(email))
			throw new NullReferenceException("Email cannot be empty.");

		var member = await db.Members
			.Where(m => m.Email == email)
			.SingleOrDefaultAsync();

		if (member != null)
			return member;

		using var transaction = await db.Database.BeginTransactionAsync();

		try
		{
			// Create member
			member = new MemberEntity
			{
				Email = email,
				IsActive = true,
			};
			db.Members.Add(member);
			await db.SaveChangesAsync();

			await AddSampleData(db, member.Id);

			await transaction.CommitAsync();
			return member;
		}
		catch
		{
			await transaction.RollbackAsync();
			return null;
		}
	}

	public static async Task<Member> GetMember(this KitchenDbContext db, MemberEntity memberEntity)
	{
		var member = new Member
		{
			AppTheme = memberEntity.AppTheme,
			DateCreatedUtc = memberEntity.DateCreatedUtc,
			DateLastModifiedUtc = memberEntity.DateLastModifiedUtc,
			Email = memberEntity.Email,
			Sid = memberEntity.Id.Encode(),
			IsActive = memberEntity.IsActive,
		};

		member.Settings = await db.GetMemberSettings(memberEntity.Id);

		long cookbookId = await db.MemberMeta.GetLongMeta(memberEntity.Id, KitchenGlobals.MetaKeys.ActiveCookbookId);
		if (cookbookId > 0)
		{
			member.ActiveCookbook = await db.CookbookMembers
				.AsNoTracking()
				.Include(x => x.Cookbook)
				.Where(x => x.MemberId == memberEntity.Id && x.CookbookId == cookbookId)
				.Select(x => new MemberCookbook
				{
					CookbookSid = x.CookbookId.Encode(),
					PermissionLevel = x.PermissionLevel,
					Title = x.Cookbook.Title,
				})
				.SingleOrDefaultAsync();
		}

		long householdId = await db.MemberMeta.GetLongMeta(memberEntity.Id, KitchenGlobals.MetaKeys.ActiveHouseholdId);
		if (householdId > 0)
		{
			member.ActiveHousehold = await db.HouseholdMembers
				.AsNoTracking()
				.Include(x => x.Household)
				.Where(x => x.MemberId == memberEntity.Id && x.HouseholdId == householdId)
				.Select(x => new MemberHousehold
				{
					HouseholdSid = x.HouseholdId.Encode(),
					PermissionLevel = x.PermissionLevel,
					Title = x.Household.Title,
				})
				.SingleOrDefaultAsync();
		}

		return member;
	}

	public static async Task<AppSettings> GetMemberSettings(this KitchenDbContext db, long memberId)
	{
		string? settingsJson = await db.MemberMeta.GetMeta(memberId, KitchenGlobals.MetaKeys.AppSettings);

		if (!string.IsNullOrEmpty(settingsJson))
		{
			return JsonSerializer.Deserialize<AppSettings>(settingsJson) ?? new();
		}

		return new();
	}

	public static async Task SaveMemberSettings(this KitchenDbContext db, long memberId, AppSettings settings)
	{
		string settingsJson = JsonSerializer.Serialize(settings);
		await db.SaveMeta(memberId, KitchenGlobals.MetaKeys.AppSettings, settingsJson);
	}

	private static async Task AddSampleData(KitchenDbContext db, long memberId)
	{
		HouseholdEntity household = new()
		{
			MemberId = memberId,
			Title = "My Household"
		};
		db.Households.Add(household);
		await db.SaveChangesAsync();

		db.HouseholdMembers.Add(new HouseholdMemberEntity
		{
			HouseholdId = household.Id,
			MemberId = memberId,
			PermissionLevel = PermissionLevels.Admin
		});
		await db.SaveChangesAsync();

		await db.SaveMeta(memberId, KitchenGlobals.MetaKeys.ActiveHouseholdId, household.Id.ToString());

		RecipeEntity recipe = new()
		{
			CookTimeMinutes = 15,
			Description = "A simple, four ingredient dessert.",
			HouseholdId = household.Id,
			PrepTimeMinutes = 10,
			Title = "Banana Oat Cookies",
			TotalServings = 12
		};
		db.Recipes.Add(recipe);
		await db.SaveChangesAsync();

		RecipeIngredientGroupEntity ingredientGroup = new()
		{
			HouseholdId = household.Id,
			RecipeId = recipe.Id,
			SortOrder = 0,
			Title = string.Empty
		};
		db.RecipeIngredientGroups.Add(ingredientGroup);
		await db.SaveChangesAsync();

		db.RecipeIngredients.AddRange(
			new RecipeIngredientEntity
			{
				Amount = 2,
				AmountLabel = "large",
				AmountN = 2,
				AmountText = "2",
				AmountUnit = MeasurementUnits.Item,
				HouseholdId = household.Id,
				Key = "bananas",
				ListGroupId = ingredientGroup.Id,
				PrepNote = "well-ripened",
				RecipeId = recipe.Id,
				SortOrder = 0,
				Title = "bananas"
			},
			new RecipeIngredientEntity
			{
				Amount = 1,
				AmountLabel = "cup",
				AmountN = 236.59M,
				AmountText = "1",
				AmountUnit = MeasurementUnits.cup,
				HouseholdId = household.Id,
				Key = "old-fashioned-rolled-oats",
				ListGroupId = ingredientGroup.Id,
				RecipeId = recipe.Id,
				SortOrder = 1,
				Title = "old fashioned rolled oats"
			},
			new RecipeIngredientEntity
			{
				Amount = 1,
				AmountLabel = "tsp",
				AmountN = 4.93M,
				AmountText = "1",
				AmountUnit = MeasurementUnits.tsp,
				HouseholdId = household.Id,
				Key = "cinnamon",
				ListGroupId = ingredientGroup.Id,
				RecipeId = recipe.Id,
				SortOrder = 2,
				Title = "cinnamon"
			},
			new RecipeIngredientEntity
			{
				Amount = 0.25M,
				AmountLabel = "cup",
				AmountN = 59.148M,
				AmountText = "1/4",
				AmountUnit = MeasurementUnits.cup,
				HouseholdId = household.Id,
				Key = "walnuts",
				ListGroupId = ingredientGroup.Id,
				PrepNote = "chopped (optional)",
				RecipeId = recipe.Id,
				SortOrder = 3,
				Title = "walnuts"
			}
		);
		await db.SaveChangesAsync();

		db.RecipeSteps.AddRange(
			new RecipeStepEntity
			{
				HouseholdId = household.Id,
				RecipeId = recipe.Id,
				SortOrder = 0,
				Instructions = "Preheat oven to 350°F."
			},
			new RecipeStepEntity
			{
				HouseholdId = household.Id,
				RecipeId = recipe.Id,
				SortOrder = 1,
				Instructions = "Mash the bananas in a medium bowl."
			},
			new RecipeStepEntity
			{
				HouseholdId = household.Id,
				RecipeId = recipe.Id,
				SortOrder = 2,
				Instructions = "Add the oats and walnuts and mix thoroughly."
			},
			new RecipeStepEntity
			{
				HouseholdId = household.Id,
				RecipeId = recipe.Id,
				SortOrder = 3,
				Instructions = "Shape into balls with your hands and place on a baking pan covered with parchment paper (or greased pan). Press down into a disc shape if desired."
			},
			new RecipeStepEntity
			{
				HouseholdId = household.Id,
				RecipeId = recipe.Id,
				SortOrder = 4,
				Instructions = "Bake for 12-15 minutes."
			}
		);
		await db.SaveChangesAsync();

		TagEntity tag1 = new()
		{
			HouseholdId = household.Id,
			Key = "vegan",
			Title = "Vegan"
		};

		TagEntity tag2 = new()
		{
			HouseholdId = household.Id,
			Key = "vegetarian",
			Title = "Vegetarian"
		};

		TagEntity tag3 = new()
		{
			HouseholdId = household.Id,
			Key = "desserts",
			Title = "Desserts"
		};

		db.Tags.AddRange(tag1, tag2, tag3);
		await db.SaveChangesAsync();

		db.RecipeTags.AddRange(
			new RecipeTagEntity
			{
				RecipeId = recipe.Id,
				TagId = tag1.Id,
			},
			new RecipeTagEntity
			{
				RecipeId = recipe.Id,
				TagId = tag2.Id,
			},
			new RecipeTagEntity
			{
				RecipeId = recipe.Id,
				TagId = tag3.Id,
			}
		);
		await db.SaveChangesAsync();

		ProductCategoryEntity pcFruitVeg = new()
		{
			HouseholdId = household.Id,
			Title = "Fruits & Vegetables"
		};

		ProductCategoryEntity pcSpices = new()
		{
			HouseholdId = household.Id,
			Title = "Spices"
		};

		ProductCategoryEntity pcNutsSeeds = new()
		{
			HouseholdId = household.Id,
			Title = "Nuts & Seeds"
		};

		ProductCategoryEntity pcBreakfastCereal = new()
		{
			HouseholdId = household.Id,
			Title = "Breakfast & Cereal"
		};

		db.ProductCategories.AddRange(pcFruitVeg, pcSpices, pcNutsSeeds, pcBreakfastCereal);
		await db.SaveChangesAsync();

		ProductEntity pBananas = new()
		{
			Brand = "Chiquita",
			HasNutritionInfo = false,
			HouseholdId = household.Id,
			ProductCategoryId = pcFruitVeg.Id,
			Title = "Banana"
		};

		ProductEntity pOats = new()
		{
			Brand = "Quaker",
			Calories = 150,
			Description = "NET WT 42oz (1.19kg)",
			DietaryFiber = 4,
			DietaryFiberN = NutritionUnits.g.NormalizeAmount(4),
			DietaryFiberUnit = NutritionUnits.g,
			HasNutritionInfo = true,
			HouseholdId = household.Id,
			MonounsaturatedFat = 1,
			MonounsaturatedFatN = NutritionUnits.g.NormalizeAmount(1),
			MonounsaturatedFatUnit = NutritionUnits.g,
			PolyunsaturatedFat = 1,
			PolyunsaturatedFatN = NutritionUnits.g.NormalizeAmount(1),
			PolyunsaturatedFatUnit = NutritionUnits.g,
			ProductCategoryId = pcBreakfastCereal.Id,
			Protein = 5,
			ProteinN = NutritionUnits.g.NormalizeAmount(5),
			ProteinUnit = NutritionUnits.g,
			SaturatedFat = 0.5M,
			SaturatedFatN = NutritionUnits.g.NormalizeAmount(0.5M),
			SaturatedFatUnit = NutritionUnits.g,
			ServingSize = 0.5M,
			ServingSizeN = 118.295M,
			ServingSizeUnit = ServingSizeUnits.cup,
			ServingSizeUnitLabel = "cup",
			ServingSizeMetric = 40,
			ServingSizeMetricN = ServingSizeMetricUnits.g.NormalizeAmount(40),
			ServingSizeMetricUnit = ServingSizeMetricUnits.g,
			ServingsPerContainer = 30,
			SolubleFiber = 2,
			SolubleFiberN = NutritionUnits.g.NormalizeAmount(2),
			SolubleFiberUnit = NutritionUnits.g,
			Title = "Old Fashioned Whole Grain Oats",
			TotalCarbohydrates = 27,
			TotalCarbohydratesN = NutritionUnits.g.NormalizeAmount(27),
			TotalCarbohydratesUnit = NutritionUnits.g,
			TotalFat = 3,
			TotalFatN = NutritionUnits.g.NormalizeAmount(3M),
			TotalFatUnit = NutritionUnits.g,
			TotalSugars = 1,
			TotalSugarsN = NutritionUnits.g.NormalizeAmount(1),
			TotalSugarsUnit = NutritionUnits.g
		};

		ProductEntity pCinnamon = new()
		{
			Brand = "McCormick",
			Description = "NET WT 7.12oz (201g)",
			HasNutritionInfo = true,
			HouseholdId = household.Id,
			ProductCategoryId = pcBreakfastCereal.Id,
			ServingSize = 1,
			ServingSizeN = ServingSizeUnits.tsp.NormalizeAmount(1),
			ServingSizeUnit = ServingSizeUnits.tsp,
			ServingSizeUnitLabel = "tsp",
			ServingSizeMetric = 3,
			ServingSizeMetricN = ServingSizeMetricUnits.g.NormalizeAmount(3),
			ServingSizeMetricUnit = ServingSizeMetricUnits.g,
			ServingsPerContainer = 67,
			Title = "Ground Cinnamon"
		};

		ProductEntity pWalnuts = new()
		{
			HasNutritionInfo = false,
			HouseholdId = household.Id,
			ProductCategoryId = pcNutsSeeds.Id,
			Title = "Walnuts"
		};

		db.Products.AddRange(pBananas, pOats, pCinnamon, pWalnuts);
		await db.SaveChangesAsync();

		db.ProductSubstitutions.AddRange(
			new ProductSubstitutionEntity
			{
				HouseholdId = household.Id,
				IngredientKey = "bananas",
				IsPrimary = true,
				ProductId = pBananas.Id
			},
			new ProductSubstitutionEntity
			{
				HouseholdId = household.Id,
				IngredientKey = "old-fashioned-rolled-oats",
				IsPrimary = true,
				ProductId = pOats.Id
			},
			new ProductSubstitutionEntity
			{
				HouseholdId = household.Id,
				IngredientKey = "cinnamon",
				IsPrimary = true,
				ProductId = pCinnamon.Id
			},
			new ProductSubstitutionEntity
			{
				HouseholdId = household.Id,
				IngredientKey = "walnuts",
				IsPrimary = true,
				ProductId = pWalnuts.Id
			}
		);
		await db.SaveChangesAsync();

		StoreEntity store = new()
		{
			Description = "A sample store for demo purposes. You can modify or delete it as needed.",
			HouseholdId = household.Id,
			Title = "Sample Store"
		};
		db.Stores.Add(store);
		await db.SaveChangesAsync();

		StoreLocationEntity slocProduce = new()
		{
			HouseholdId = household.Id,
			SortOrder = 0,
			StoreId = store.Id,
			Title = "Fresh Produce"
		};

		StoreLocationEntity slocAisle1 = new()
		{
			Description = "Breakfast Goods & Cereals",
			HouseholdId = household.Id,
			SortOrder = 1,
			StoreId = store.Id,
			Title = "Aisle 1"
		};

		StoreLocationEntity slocAisle2 = new()
		{
			Description = "Baking Supplies & Spices",
			HouseholdId = household.Id,
			SortOrder = 2,
			StoreId = store.Id,
			Title = "Aisle 2"
		};

		db.StoreLocations.AddRange(slocProduce, slocAisle1, slocAisle2);
		await db.SaveChangesAsync();

		db.StoreProductLocations.AddRange(
			new StoreProductLocationEntity
			{
				ProductId = pBananas.Id,
				SortOrder = 0,
				StoreId= store.Id,
				StoreLocationId = slocProduce.Id
			},
			new StoreProductLocationEntity
			{
				ProductId = pOats.Id,
				SortOrder = 0,
				StoreId = store.Id,
				StoreLocationId = slocAisle1.Id
			},
			new StoreProductLocationEntity
			{
				ProductId = pCinnamon.Id,
				SortOrder = 0,
				StoreId = store.Id,
				StoreLocationId = slocAisle2.Id
			},
			new StoreProductLocationEntity
			{
				ProductId = pWalnuts.Id,
				SortOrder = 1,
				StoreId = store.Id,
				StoreLocationId = slocAisle2.Id
			}
		);
		await db.SaveChangesAsync();

		ShoppingListEntity shoppingList = new()
		{
			HouseholdId = household.Id,
			StoreId = store.Id,
			Title = "Sample Shopping List"
		};
		db.ShoppingLists.Add(shoppingList);
		await db.SaveChangesAsync();

		db.ShoppingListItems.AddRange(
			new ShoppingListItemEntity
			{
				ProductId = pBananas.Id,
				Quantity = 2,
				ShoppingListId = shoppingList.Id,
			},
			new ShoppingListItemEntity
			{
				ProductId = pOats.Id,
				Quantity = 1,
				ShoppingListId = shoppingList.Id,
			},
			new ShoppingListItemEntity
			{
				ProductId = pCinnamon.Id,
				Quantity = 1,
				ShoppingListId = shoppingList.Id,
			},
			new ShoppingListItemEntity
			{
				ProductId = pWalnuts.Id,
				Quantity = 1,
				ShoppingListId = shoppingList.Id,
			}
		);
		await db.SaveChangesAsync();

		db.ShoppingListRecipeItems.AddRange(
			new ShoppingListRecipeItemEntity
			{
				IngredientAmount = 2,
				IngredientKey = "bananas",
				PortionUsed = 1,
				ProductId = pBananas.Id,
				RecipeId = recipe.Id,
				ShoppingListId = shoppingList.Id,
				IngredientAmountUnitLabel = "large"
			},
			new ShoppingListRecipeItemEntity
			{
				IngredientAmount = 1,
				IngredientKey = "old-fashioned-rolled-oats",
				PortionUsed = ServingSizeUnits.cup.NormalizeAmount(1M) / (ServingSizeUnits.cup.NormalizeAmount(0.5M) * 30),
				ProductId = pOats.Id,
				RecipeId = recipe.Id,
				ShoppingListId = shoppingList.Id,
				IngredientAmountUnitLabel = "cup"
			},
			new ShoppingListRecipeItemEntity
			{
				IngredientAmount = 1,
				IngredientKey = "cinnamon",
				PortionUsed = ServingSizeUnits.tsp.NormalizeAmount(1M) / (ServingSizeUnits.tsp.NormalizeAmount(1) * 67),
				ProductId = pCinnamon.Id,
				RecipeId = recipe.Id,
				ShoppingListId = shoppingList.Id,
				IngredientAmountUnitLabel = "tsp"
			},
			new ShoppingListRecipeItemEntity
			{
				IngredientAmount = 0.25M,
				IngredientKey = "walnuts",
				PortionUsed = 1,
				ProductId = pWalnuts.Id,
				RecipeId = recipe.Id,
				ShoppingListId = shoppingList.Id,
				IngredientAmountUnitLabel = "cup"
			}
		);
		await db.SaveChangesAsync();
	}
}
