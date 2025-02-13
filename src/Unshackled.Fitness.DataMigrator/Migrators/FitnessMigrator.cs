using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.DataMigrator.Configuration;
using Unshackled.Fitness.DataMigrator.Enums;
using Unshackled.Fitness.DataMigrator.Extensions;

namespace Unshackled.Fitness.DataMigrator.Migrators;

internal class FitnessMigrator
{
	protected const int pageSize = 50;

	public BaseDbContext dbLegacy = default!;
	public BaseDbContext dbNew = default!;
	protected bool isIdentityInsertRequired;

	public FitnessMigrator(MigrationConfiguration mConfig, MigrationTypes migrationType)
	{
		isIdentityInsertRequired = false;
		switch (migrationType)
		{
			case MigrationTypes.MsSqlToMySql:
				dbLegacy = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MsSqlToPostgreSql:
				dbLegacy = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MsSqlToSqlite:
				dbLegacy = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.MySqlToMsSql:
				dbLegacy = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.MySqlToPostgreSql:
				dbLegacy = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.MySqlToSqlite:
				dbLegacy = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.PostgreSqlToMsSql:
				dbLegacy = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.PostgreSqlToMySql:
				dbLegacy = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.PostgreSqlToSqlite:
				dbLegacy = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.SourceTablePrefix);
				dbNew = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.DestinationTablePrefix);
				PrepareSqlitePath(mConfig.SqliteDatabase);
				break;
			case MigrationTypes.SqliteToMsSql:
				dbLegacy = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = MsSqlServerDbContext.Create(mConfig.MsSqlDatabase, mConfig.DestinationTablePrefix);
				isIdentityInsertRequired = true;
				break;
			case MigrationTypes.SqliteToMySql:
				dbLegacy = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = MySqlServerDbContext.Create(mConfig.MySqlDatabase, mConfig.DestinationTablePrefix);
				break;
			case MigrationTypes.SqliteToPostgreSql:
				dbLegacy = SqliteDbContext.Create(mConfig.SqliteDatabase, mConfig.SourceTablePrefix);
				dbNew = PostgreSqlServerDbContext.Create(mConfig.PostgreSqlDatabase, mConfig.DestinationTablePrefix);
				break;
			default:
				dbLegacy = default!;
				dbNew = default!;
				break;
		}
	}

	public async Task Migrate()
	{
		await dbNew.Database.MigrateAsync();

		await MigrateUsers();
		await MigrateUserClaims();
		await MigrateUserLogins();
		await MigrateUserTokens();
		await MigrateRoles();
		await MigrateRoleClaims();
		await MigrateUserRoles();
		await MigrateDbSet(dbLegacy.Members, dbNew.Members, "Members");
		await MigrateMemberMeta();
		await MigrateDbSet(dbLegacy.Exercises, dbNew.Exercises, "Exercises");
		await MigrateDbSet(dbLegacy.WorkoutTemplates, dbNew.WorkoutTemplates, "Workout Templates");
		await MigrateDbSet(dbLegacy.WorkoutTemplateSetGroups, dbNew.WorkoutTemplateSetGroups, "Workout Template Set Groups");
		await MigrateDbSet(dbLegacy.WorkoutTemplateSets, dbNew.WorkoutTemplateSets, "Workout Template Sets");
		await MigrateDbSet(dbLegacy.WorkoutTemplateTasks, dbNew.WorkoutTemplateTasks, "Workout Template Tasks");
		await MigrateDbSet(dbLegacy.Programs, dbNew.Programs, "Programs");
		await MigrateDbSet(dbLegacy.ProgramTemplates, dbNew.ProgramTemplates, "Program Templates");
		await MigrateDbSet(dbLegacy.Workouts, dbNew.Workouts, "Workouts");
		await MigrateDbSet(dbLegacy.WorkoutSetGroups, dbNew.WorkoutSetGroups, "Workout Set Groups");
		await MigrateDbSet(dbLegacy.WorkoutSets, dbNew.WorkoutSets, "Workout Sets");
		await MigrateDbSet(dbLegacy.WorkoutTasks, dbNew.WorkoutTasks, "Workout Tasks");
		await MigrateDbSet(dbLegacy.ActivityTypes, dbNew.ActivityTypes, "Activity Types"); ;
		await MigrateDbSet(dbLegacy.TrainingSessions, dbNew.TrainingSessions, "Training Sessions");
		await MigrateDbSet(dbLegacy.TrainingPlans, dbNew.TrainingPlans, "Training Plans");
		await MigrateDbSet(dbLegacy.TrainingPlanSessions, dbNew.TrainingPlanSessions, "Training Plan Sessions");
		await MigrateDbSet(dbLegacy.Activities, dbNew.Activities, "Activities");
		await MigrateDbSet(dbLegacy.MetricDefinitionGroups, dbNew.MetricDefinitionGroups, "Metric Definition Groups");
		await MigrateDbSet(dbLegacy.MetricDefinitions, dbNew.MetricDefinitions, "Metric Definitions");
		await MigrateDbSet(dbLegacy.Metrics, dbNew.Metrics, "Metrics");
		await MigrateDbSet(dbLegacy.MetricPresets, dbNew.MetricPresets, "Metric Presets");
		await MigrateDbSet(dbLegacy.Cookbooks, dbNew.Cookbooks, "Cookbooks");
		await MigrateCookbookMembers("Cookbook Members");
		await MigrateDbSet(dbLegacy.CookbookInvites, dbNew.CookbookInvites, "Cookbook Invites");
		await MigrateDbSet(dbLegacy.Households, dbNew.Households, "Households");
		await MigrateHouseholdMembers("Household Members");
		await MigrateDbSet(dbLegacy.HouseholdInvites, dbNew.HouseholdInvites, "Household Invites");
		await MigrateDbSet(dbLegacy.ProductCategories, dbNew.ProductCategories, "Product Categories");
		await MigrateDbSet(dbLegacy.Products, dbNew.Products, "Products");
		await MigrateDbSet(dbLegacy.ProductBundles, dbNew.ProductBundles, "Product Bundles");
		await MigrateProductBundleItems("Product Bundle Items");
		await MigrateDbSet(dbLegacy.Tags, dbNew.Tags, "Tags");
		await MigrateDbSet(dbLegacy.Recipes, dbNew.Recipes, "Recipes");
		await MigrateDbSet(dbLegacy.RecipeIngredientGroups, dbNew.RecipeIngredientGroups, "Recipe Ingredient Groups");
		await MigrateDbSet(dbLegacy.RecipeIngredients, dbNew.RecipeIngredients, "Recipe Ingredients");
		await MigrateDbSet(dbLegacy.RecipeSteps, dbNew.RecipeSteps, "Recipe Steps");
		await MigrateDbSet(dbLegacy.RecipeNotes, dbNew.RecipeNotes, "Recipe Notes");
		await MigrateDbSet(dbLegacy.RecipeImages, dbNew.RecipeImages, "Recipe Images");
		await MigrateRecipeTags("Recipe Tags");
		await MigrateProductSubstitutions("Product Substitutions");
		await MigrateDbSet(dbLegacy.Stores, dbNew.Stores, "Stores");
		await MigrateDbSet(dbLegacy.StoreLocations, dbNew.StoreLocations, "Store Locations");
		await MigrateStoreProductLocations("Store Product Locations");
		await MigrateDbSet(dbLegacy.ShoppingLists, dbNew.ShoppingLists, "Shopping Lists");
		await MigrateShoppingListItems("Shopping List Items");
		await MigrateShoppingListRecipeItems("Shopping List Recipe Items");
		await MigrateDbSet(dbLegacy.MealDefinitions, dbNew.MealDefinitions, "Meal Definitions");
		await MigrateDbSet(dbLegacy.MealPlanRecipes, dbNew.MealPlanRecipes, "Meal Plan Recipes");
		await MigrateCookbookRecipes("Cookbook Recipes");
	}

	protected async Task MigrateDbSet<TEntity>(DbSet<TEntity> dbSource, DbSet<TEntity> dbDest, string title)
		where TEntity : BaseEntity
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbSource.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbSource
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbDest.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<TEntity>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateCookbookMembers(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.CookbookMembers.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.CookbookMembers
					.AsNoTracking()
					.OrderBy(x => x.CookbookId)
						.ThenBy(x => x.MemberId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.CookbookMembers.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateCookbookRecipes(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.CookbookRecipes.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.CookbookRecipes
					.AsNoTracking()
					.OrderBy(x => x.CookbookId)
						.ThenBy(x => x.RecipeId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.CookbookRecipes.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateHouseholdMembers(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.HouseholdMembers.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.HouseholdMembers
					.AsNoTracking()
					.OrderBy(x => x.HouseholdId)
						.ThenBy(x => x.MemberId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.HouseholdMembers.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateMemberMeta()
	{
		string title = "Member Meta";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.MemberMeta.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.MemberMeta
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.MemberMeta.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<MemberMetaEntity>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateProductBundleItems(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.ProductBundleItems.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.ProductBundleItems
					.AsNoTracking()
					.OrderBy(x => x.ProductBundleId)
						.ThenBy(x => x.ProductId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.ProductBundleItems.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateProductSubstitutions(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.ProductSubstitutions.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.ProductSubstitutions
					.AsNoTracking()
					.OrderBy(x => x.IngredientKey)
						.ThenBy(x => x.ProductId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.ProductSubstitutions.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateRecipeTags(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.RecipeTags.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.RecipeTags
					.AsNoTracking()
					.OrderBy(x => x.RecipeId)
						.ThenBy(x => x.TagId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.RecipeTags.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateRoleClaims()
	{
		string title = "Role Claims";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.RoleClaims.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.RoleClaims
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.RoleClaims.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityRoleClaim<string>>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateRoles()
	{
		string title = "Roles";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.Roles.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.Roles
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.Roles.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityRole>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateStoreProductLocations(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.StoreProductLocations.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.StoreProductLocations
					.AsNoTracking()
					.OrderBy(x => x.StoreId)
						.ThenBy(x => x.ProductId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.StoreProductLocations.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateShoppingListItems(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.ShoppingListItems.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.ShoppingListItems
					.AsNoTracking()
					.OrderBy(x => x.ShoppingListId)
						.ThenBy(x => x.ProductId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.ShoppingListItems.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateShoppingListRecipeItems(string title)
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.ShoppingListRecipeItems.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.ShoppingListRecipeItems
					.AsNoTracking()
					.OrderBy(x => x.ShoppingListId)
						.ThenBy(x => x.ProductId)
						.ThenBy(x => x.RecipeId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.ShoppingListRecipeItems.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserRoles()
	{
		string title = "User Roles";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserRoles.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserRoles
					.AsNoTracking()
					.OrderBy(x => x.UserId)
						.ThenBy(x => x.RoleId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserRoles.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserClaims()
	{
		string title = "User Claims";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserClaims.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserClaims
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserClaims.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityUserClaim<string>>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserLogins()
	{
		string title = "Users";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserLogins.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserLogins
					.AsNoTracking()
					.OrderBy(x => x.LoginProvider)
						.ThenBy(x => x.ProviderKey)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserLogins.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUsers()
	{
		string title = "Users";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.Users.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.Users
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.Users.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserTokens()
	{
		string title = "User Tokens";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserTokens.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserTokens
					.AsNoTracking()
					.OrderBy(x => x.UserId)
						.ThenBy(x => x.LoginProvider)
							.ThenBy(x => x.Name)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserTokens.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected void PrepareSqlitePath(string connString)
	{
		Msg.WriteOngoing("Preparing directories");

		string path = connString
			.Replace("Data Source=", string.Empty);

		if (Path.DirectorySeparatorChar != '/')
		{
			path = path.Replace('/', Path.DirectorySeparatorChar);
		}

		int idxLastSlash = path.LastIndexOf(Path.DirectorySeparatorChar);
		string dirPath = path.Substring(0, idxLastSlash + 1);

		Directory.CreateDirectory(dirPath);

		if (File.Exists(path))
		{
			File.Delete(path);
		}
		Msg.WriteComplete();
	}
}
