using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class ShoppingListRecipeItemEntity
{
	public long ShoppingListId { get; set; }
	public virtual ShoppingListEntity ShoppingList { get; set; } = default!;
	public long ProductId { get; set; }
	public virtual ProductEntity Product { get; set; } = default!;
	public long RecipeId { get; set; }
	public RecipeEntity Recipe { get; set; } = default!;
	public string IngredientKey { get; set; } = string.Empty;
	public decimal Amount { get; set; }
	public string UnitLabel { get; set; } = string.Empty;
	public decimal PortionUsed { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<ShoppingListRecipeItemEntity>
	{
		public void Configure(EntityTypeBuilder<ShoppingListRecipeItemEntity> config)
		{
			config.ToTable("ShoppingListRecipeItems")
				.HasKey(x => new { x.ShoppingListId, x.ProductId, x.RecipeId });

			config.Property(x => x.Amount)
				.HasPrecision(8, 3);

			config.Property(x => x.PortionUsed)
				.HasPrecision(15, 10);

			config.HasOne(x => x.ShoppingList)
				.WithMany(x => x.RecipeItems)
				.HasForeignKey(x => x.ShoppingListId);

			config.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);

			config.HasOne(x => x.Recipe)
				.WithMany()
				.HasForeignKey(x => x.RecipeId);
		}
	}
}
