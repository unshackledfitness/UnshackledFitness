using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class ProductCategoryEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;

	public virtual List<ProductEntity> Products { get; set; } = [];

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<ProductCategoryEntity>, IEntityTypeConfiguration<ProductCategoryEntity>
	{
		public override void Configure(EntityTypeBuilder<ProductCategoryEntity> config)
		{
			base.Configure(config);

			config.ToTable("ProductCategories");

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => x.Title);
		}
	}
}
