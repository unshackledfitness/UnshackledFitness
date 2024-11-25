using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class ShoppingListEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;
	public long? StoreId { get; set; }
	public virtual StoreEntity? Store { get; set; }

	public virtual List<ShoppingListItemEntity> Items { get; set; } = [];
	public virtual List<ShoppingListRecipeItemEntity> RecipeItems { get; set; } = [];

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<ShoppingListEntity>, IEntityTypeConfiguration<ShoppingListEntity>
	{
		public override void Configure(EntityTypeBuilder<ShoppingListEntity> config)
		{
			base.Configure(config);

			config.ToTable("ShoppingLists");

			config.Property(x => x.StoreId)
				.IsRequired(false);

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => new { x.HouseholdId, x.Title });
		}
	}
}
