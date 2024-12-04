using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class ProductBundleItemEntity
{
	public long ProductBundleId { get; set; }
	public virtual ProductBundleEntity ProductBundle { get; set; } = default!;
	public long ProductId { get; set; }
	public virtual ProductEntity Product { get; set; } = default!;
	public int Quantity { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<ProductBundleItemEntity>
	{
		public void Configure(EntityTypeBuilder<ProductBundleItemEntity> config)
		{
			config.ToTable("ProductBundleItems")
				.HasKey(x => new { x.ProductBundleId, x.ProductId });

			config.HasOne(x => x.ProductBundle)
				.WithMany(x => x.Products)
				.HasForeignKey(x => x.ProductBundleId);

			config.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);
		}
	}
}
