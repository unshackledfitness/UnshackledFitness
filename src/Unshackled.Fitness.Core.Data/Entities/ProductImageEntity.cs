using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ProductImageEntity : BaseHouseholdEntity
{
	public long ProductId { get; set; }
	public virtual ProductEntity Product { get; set; } = default!;
	public string Container { get; set; } = string.Empty;
	public string RelativePath { get; set; } = string.Empty;
	public string MimeType { get; set; } = string.Empty;
	public long FileSize { get; set; }
	public bool IsFeatured { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<ProductImageEntity>, IEntityTypeConfiguration<ProductImageEntity>
	{
		public override void Configure(EntityTypeBuilder<ProductImageEntity> config)
		{
			base.Configure(config);

			config.ToTable("ProductImages");

			config.Property(x => x.Container)
				.HasMaxLength(50)
				.IsRequired();

			config.Property(x => x.RelativePath)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.MimeType)
				.HasMaxLength(50)
				.IsRequired();

			config.HasOne(x => x.Product)
				.WithMany(x => x.Images)
				.HasForeignKey(x => x.ProductId);

			config.HasIndex(x => x.ProductId);
		}
	}
}
