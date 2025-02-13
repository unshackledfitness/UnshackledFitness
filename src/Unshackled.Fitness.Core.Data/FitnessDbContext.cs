using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Fitness.Core.Data;

public class FitnessDbContext : BaseDbContext
{
	public FitnessDbContext(DbContextOptions<FitnessDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	public FitnessDbContext(DbContextOptions options, 
		ConnectionStrings connectionStrings, 
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

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
	public DbSet<MealDefinitionEntity> MealDefinitions => Set<MealDefinitionEntity>();
	public DbSet<MealPlanRecipeEntity> MealPlanRecipes => Set<MealPlanRecipeEntity>();
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

	protected override void OnModelCreating(ModelBuilder builder)
	{
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
		builder.ApplyConfiguration(new MealDefinitionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MealPlanRecipeEntity.TypeConfiguration());
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

		base.OnModelCreating(builder);
	}
}
