using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class ProductBundleEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;

	public virtual List<ProductBundleItemEntity> Products { get; set; } = new();

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<ProductBundleEntity>, IEntityTypeConfiguration<ProductBundleEntity>
	{
		public override void Configure(EntityTypeBuilder<ProductBundleEntity> config)
		{
			base.Configure(config);

			config.ToTable("ProductBundles");

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => new { x.HouseholdId, x.Title });
		}
	}
}
