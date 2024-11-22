using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Food.Core.Data;

public class FoodDbContext : BaseDbContext
{
	public FoodDbContext(DbContextOptions<FoodDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	public FoodDbContext(DbContextOptions options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	public DbSet<CookbookEntity> Cookbooks => Set<CookbookEntity>();
	public DbSet<CookbookInviteEntity> CookbookInvites => Set<CookbookInviteEntity>();
	public DbSet<CookbookMemberEntity> CookbookMembers => Set<CookbookMemberEntity>();
	public DbSet<CookbookRecipeEntity> CookbookRecipes => Set<CookbookRecipeEntity>();
	public DbSet<HouseholdEntity> Households => Set<HouseholdEntity>();
	public DbSet<HouseholdInviteEntity> HouseholdInvites => Set<HouseholdInviteEntity>();
	public DbSet<HouseholdMemberEntity> HouseholdMembers => Set<HouseholdMemberEntity>();
	public DbSet<ProductBundleItemEntity> ProductBundleItems => Set<ProductBundleItemEntity>();
	public DbSet<ProductBundleEntity> ProductBundles => Set<ProductBundleEntity>();
	public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
	public DbSet<ProductEntity> Products => Set<ProductEntity>();
	public DbSet<ProductSubstitutionEntity> ProductSubstitutions => Set<ProductSubstitutionEntity>();
	public DbSet<RecipeEntity> Recipes => Set<RecipeEntity>();
	public DbSet<RecipeIngredientGroupEntity> RecipeIngredientGroups => Set<RecipeIngredientGroupEntity>();
	public DbSet<RecipeIngredientEntity> RecipeIngredients => Set<RecipeIngredientEntity>();
	public DbSet<RecipeNoteEntity> RecipeNotes => Set<RecipeNoteEntity>();
	public DbSet<RecipeStepEntity> RecipeSteps => Set<RecipeStepEntity>();
	public DbSet<RecipeStepIngredientEntity> RecipeStepIngredients => Set<RecipeStepIngredientEntity>();
	public DbSet<ShoppingListEntity> ShoppingLists => Set<ShoppingListEntity>();
	public DbSet<ShoppingListItemEntity> ShoppingListItems => Set<ShoppingListItemEntity>();
	public DbSet<StoreLocationEntity> StoreLocations => Set<StoreLocationEntity>();
	public DbSet<StoreEntity> Stores => Set<StoreEntity>();
	public DbSet<StoreProductLocationEntity> StoreProductLocations => Set<StoreProductLocationEntity>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new CookbookEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookInviteEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookMemberEntity.TypeConfiguration());
		builder.ApplyConfiguration(new CookbookRecipeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new HouseholdEntity.TypeConfiguration());
		builder.ApplyConfiguration(new HouseholdInviteEntity.TypeConfiguration());
		builder.ApplyConfiguration(new HouseholdMemberEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductBundleEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductBundleItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductCategoryEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ProductSubstitutionEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeIngredientGroupEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeIngredientEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeNoteEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeStepEntity.TypeConfiguration());
		builder.ApplyConfiguration(new RecipeStepIngredientEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreLocationEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreProductLocationEntity.TypeConfiguration());

		base.OnModelCreating(builder);
	}
}
