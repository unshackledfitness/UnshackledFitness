using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class ShoppingListItemEntity
{
	public long ShoppingListId { get; set; }
	public ShoppingListEntity ShoppingList { get; set; } = default!;
	public long ProductId { get; set; }
	public virtual ProductEntity Product { get; set; } = default!;
	public int Quantity { get; set; }
	public bool IsInCart { get; set; }
	public string? RecipeAmountsJson { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<ShoppingListItemEntity>
	{
		public void Configure(EntityTypeBuilder<ShoppingListItemEntity> config)
		{
			config.ToTable("ShoppingListItems")
				.HasKey(x => new { x.ShoppingListId, x.ProductId });

			config.HasOne(x => x.ShoppingList)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.ShoppingListId);

			config.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);
		}
	}
}
