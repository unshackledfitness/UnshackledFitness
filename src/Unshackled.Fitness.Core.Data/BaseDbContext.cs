using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data;

public class BaseDbContext : IdentityDbContext<UserEntity>
{
	protected readonly ConnectionStrings ConnectionStrings;
	protected readonly DbConfiguration DbConfig;

	public BaseDbContext(DbContextOptions<BaseDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options)
	{
		this.ConnectionStrings = connectionStrings;
		this.DbConfig = dbConfig;
	}

	protected BaseDbContext(DbContextOptions options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options)
	{
		this.ConnectionStrings = connectionStrings;
		this.DbConfig = dbConfig;
	}

	public DbSet<ActivityEntity> Activities => Set<ActivityEntity>();
	public DbSet<ActivityTypeEntity> ActivityTypes => Set<ActivityTypeEntity>();
	public DbSet<ExerciseEntity> Exercises => Set<ExerciseEntity>();
	public DbSet<CookbookEntity> Cookbooks => Set<CookbookEntity>();
	public DbSet<CookbookInviteEntity> CookbookInvites => Set<CookbookInviteEntity>();
	public DbSet<CookbookMemberEntity> CookbookMembers => Set<CookbookMemberEntity>();
	public DbSet<CookbookRecipeEntity> CookbookRecipes => Set<CookbookRecipeEntity>();
	public DbSet<HouseholdEntity> Households => Set<HouseholdEntity>();
	public DbSet<HouseholdInviteEntity> HouseholdInvites => Set<HouseholdInviteEntity>();
	public DbSet<HouseholdMemberEntity> HouseholdMembers => Set<HouseholdMemberEntity>();
	public DbSet<MealPrepPlanSlotEntity> MealPrepPlanSlots => Set<MealPrepPlanSlotEntity>();
	public DbSet<MealPrepPlanRecipeEntity> MealPrepPlanRecipes => Set<MealPrepPlanRecipeEntity>();
	public DbSet<MemberMetaEntity> MemberMeta => Set<MemberMetaEntity>();
	public DbSet<MemberEntity> Members => Set<MemberEntity>();
	public DbSet<MetricDefinitionGroupEntity> MetricDefinitionGroups => Set<MetricDefinitionGroupEntity>();
	public DbSet<MetricDefinitionEntity> MetricDefinitions => Set<MetricDefinitionEntity>();
	public DbSet<MetricPresetEntity> MetricPresets => Set<MetricPresetEntity>();
	public DbSet<MetricEntity> Metrics => Set<MetricEntity>();
	public DbSet<ProductBundleItemEntity> ProductBundleItems => Set<ProductBundleItemEntity>();
	public DbSet<ProductBundleEntity> ProductBundles => Set<ProductBundleEntity>();
	public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
	public DbSet<ProductImageEntity> ProductImages => Set<ProductImageEntity>();
	public DbSet<ProductEntity> Products => Set<ProductEntity>();
	public DbSet<ProductSubstitutionEntity> ProductSubstitutions => Set<ProductSubstitutionEntity>();
	public DbSet<ProgramEntity> Programs => Set<ProgramEntity>();
	public DbSet<ProgramTemplateEntity> ProgramTemplates => Set<ProgramTemplateEntity>();
	public DbSet<RecipeEntity> Recipes => Set<RecipeEntity>();
	public DbSet<RecipeImageEntity> RecipeImages => Set<RecipeImageEntity>();
	public DbSet<RecipeIngredientGroupEntity> RecipeIngredientGroups => Set<RecipeIngredientGroupEntity>();
	public DbSet<RecipeIngredientEntity> RecipeIngredients => Set<RecipeIngredientEntity>();
	public DbSet<RecipeNoteEntity> RecipeNotes => Set<RecipeNoteEntity>();
	public DbSet<RecipeStepEntity> RecipeSteps => Set<RecipeStepEntity>();
	public DbSet<RecipeTagEntity> RecipeTags => Set<RecipeTagEntity>();
	public DbSet<ShoppingListEntity> ShoppingLists => Set<ShoppingListEntity>();
	public DbSet<ShoppingListItemEntity> ShoppingListItems => Set<ShoppingListItemEntity>();
	public DbSet<ShoppingListRecipeItemEntity> ShoppingListRecipeItems => Set<ShoppingListRecipeItemEntity>();
	public DbSet<StoreLocationEntity> StoreLocations => Set<StoreLocationEntity>();
	public DbSet<StoreEntity> Stores => Set<StoreEntity>();
	public DbSet<StoreProductLocationEntity> StoreProductLocations => Set<StoreProductLocationEntity>();
	public DbSet<TagEntity> Tags => Set<TagEntity>();
	public DbSet<TrainingPlanEntity> TrainingPlans => Set<TrainingPlanEntity>();
	public DbSet<TrainingPlanSessionEntity> TrainingPlanSessions => Set<TrainingPlanSessionEntity>();
	public DbSet<TrainingSessionEntity> TrainingSessions => Set<TrainingSessionEntity>();
	public DbSet<WorkoutEntity> Workouts => Set<WorkoutEntity>();
	public DbSet<WorkoutSetEntity> WorkoutSets => Set<WorkoutSetEntity>();
	public DbSet<WorkoutSetGroupEntity> WorkoutSetGroups => Set<WorkoutSetGroupEntity>();
	public DbSet<WorkoutTaskEntity> WorkoutTasks => Set<WorkoutTaskEntity>();
	public DbSet<WorkoutTemplateEntity> WorkoutTemplates => Set<WorkoutTemplateEntity>();
	public DbSet<WorkoutTemplateSetEntity> WorkoutTemplateSets => Set<WorkoutTemplateSetEntity>();
	public DbSet<WorkoutTemplateSetGroupEntity> WorkoutTemplateSetGroups => Set<WorkoutTemplateSetGroupEntity>();
	public DbSet<WorkoutTemplateTaskEntity> WorkoutTemplateTasks => Set<WorkoutTemplateTaskEntity>();

	private static void ApplyDatedDefaults(EntityEntry<IDatedEntity> entry)
	{
		switch (entry.State)
		{
			case EntityState.Added:
				if (entry.Entity.DateCreatedUtc == DateTime.MinValue)
					entry.Entity.DateCreatedUtc = DateTime.UtcNow;
				break;
			case EntityState.Modified:
				entry.Entity.DateLastModifiedUtc = DateTime.UtcNow;
				break;
		}
	}

	public override int SaveChanges()
	{
		foreach (var entry in ChangeTracker.Entries<IDatedEntity>())
		{
			ApplyDatedDefaults(entry);
		}

		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<IDatedEntity>())
		{
			ApplyDatedDefaults(entry);
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfiguration(new ActivityEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ActivityTypeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookInviteEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookMemberEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookRecipeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ExerciseEntity.TypeConfiguration());
		builder.ApplyConfiguration(new HouseholdEntity.TypeConfiguration());
		builder.ApplyConfiguration(new HouseholdInviteEntity.TypeConfiguration());
		builder.ApplyConfiguration(new HouseholdMemberEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MealPrepPlanSlotEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MealPrepPlanRecipeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MemberMetaEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MemberEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricDefinitionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricDefinitionGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MetricPresetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductBundleEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductBundleItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductCategoryEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductImageEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductSubstitutionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProgramEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProgramTemplateEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeImageEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeIngredientGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeIngredientEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeNoteEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeStepEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeTagEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListRecipeItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreLocationEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreProductLocationEntity.TypeConfiguration());
		builder.ApplyConfiguration(new TagEntity.TypeConfiguration());
		builder.ApplyConfiguration(new TrainingPlanEntity.TypeConfiguration());
		builder.ApplyConfiguration(new TrainingPlanSessionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new TrainingSessionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutSetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutSetGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTaskEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateSetEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateSetGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new WorkoutTemplateTaskEntity.TypeConfiguration());

		builder.Entity<UserEntity>(x =>
		{
			x.ToTable("Users");
		});

		builder.Entity<IdentityUserClaim<string>>(x =>
		{
			x.ToTable("UserClaims");
		});

		builder.Entity<IdentityUserLogin<string>>(x =>
		{
			x.ToTable("UserLogins");
		});

		builder.Entity<IdentityUserToken<string>>(x =>
		{
			x.ToTable("UserTokens");
		});

		builder.Entity<IdentityRole>(x =>
		{
			x.ToTable("Roles");
		});

		builder.Entity<IdentityRoleClaim<string>>(x =>
		{
			x.ToTable("RoleClaims");
		});

		builder.Entity<IdentityUserRole<string>>(x =>
		{
			x.ToTable("UserRoles");
		});

		if (!string.IsNullOrEmpty(DbConfig.TablePrefix))
		{
			foreach (var entity in builder.Model.GetEntityTypes())
			{
				entity.SetTableName(DbConfig.TablePrefix + entity.GetTableName());
			}
		}

		foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
		{
			relationship.DeleteBehavior = DeleteBehavior.NoAction;
		}
	}
}
