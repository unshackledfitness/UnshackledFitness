using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class ProductSubstitutionEntity
{
	public string IngredientKey { get; set; } = string.Empty;
	public long ProductId { get; set; }
	public virtual ProductEntity Product { get; set; } = null!;
	public long HouseholdId { get; set; }
	public virtual HouseholdEntity Household { get; set; } = null!;
	public bool IsPrimary { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<ProductSubstitutionEntity>
	{
		public virtual void Configure(EntityTypeBuilder<ProductSubstitutionEntity> config)
		{
			config.ToTable("ProductSubstitutions")
				.HasKey(x => new { x.IngredientKey, x.ProductId });

			config.Property(x => x.IngredientKey)
				.HasMaxLength(255)
				.IsRequired();

			config.HasOne(x => x.Product)
				.WithMany()
				.HasForeignKey(x => x.ProductId)
				.OnDelete(DeleteBehavior.Cascade);

			config.HasOne(p => p.Household)
				.WithMany()
				.HasForeignKey(p => p.HouseholdId);

			config.HasIndex(p => new { p.HouseholdId, p.IngredientKey });
		}
	}
}
