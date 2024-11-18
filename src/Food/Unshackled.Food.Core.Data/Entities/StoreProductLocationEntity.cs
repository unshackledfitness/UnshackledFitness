using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class StoreProductLocationEntity
{
	public long StoreId { get; set; }
	public StoreEntity Store { get; set; } = default!;
	public long ProductId { get; set; }
	public ProductEntity Product { get; set; } = default!;
	public long StoreLocationId { get; set; }
	public StoreLocationEntity StoreLocation { get; set; } = default!;
	public int SortOrder { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<StoreProductLocationEntity>
	{
		public void Configure(EntityTypeBuilder<StoreProductLocationEntity> config)
		{
			config.ToTable("StoreProductLocations")
				.HasKey(x => new { x.StoreId, x.ProductId });

			config.HasOne(x => x.Store)
				.WithMany()
				.HasForeignKey(x => x.StoreId);

			config.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId);

			config.HasOne(x => x.StoreLocation)
				.WithMany()
				.HasForeignKey(x => x.StoreLocationId);

			config.HasIndex(x => new { x.StoreId, x.StoreLocationId, x.SortOrder });
		}
	}
}
