using Microsoft.EntityFrameworkCore;
using Unshackled.Kitchen.Core.Data.Entities;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Kitchen.Core.Data;

public class KitchenDbContext : BaseDbContext
{
	public KitchenDbContext(DbContextOptions<KitchenDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options, connectionStrings, dbConfig) { }

	public KitchenDbContext(DbContextOptions options,
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
	public DbSet<RecipeTagEntity> RecipeTags => Set<RecipeTagEntity>();
	public DbSet<ShoppingListEntity> ShoppingLists => Set<ShoppingListEntity>();
	public DbSet<ShoppingListItemEntity> ShoppingListItems => Set<ShoppingListItemEntity>();
	public DbSet<ShoppingListRecipeItemEntity> ShoppingListRecipeItems => Set<ShoppingListRecipeItemEntity>();
	public DbSet<StoreLocationEntity> StoreLocations => Set<StoreLocationEntity>();
	public DbSet<StoreEntity> Stores => Set<StoreEntity>();
	public DbSet<StoreProductLocationEntity> StoreProductLocations => Set<StoreProductLocationEntity>();
	public DbSet<TagEntity> Tags => Set<TagEntity>();

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
		builder.ApplyConfiguration(new RecipeTagEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new ShoppingListRecipeItemEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreLocationEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreEntity.TypeConfiguration());
		builder.ApplyConfiguration(new StoreProductLocationEntity.TypeConfiguration());
		builder.ApplyConfiguration(new TagEntity.TypeConfiguration());

		base.OnModelCreating(builder);
	}
}
