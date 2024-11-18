using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class StoreEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }

	public virtual List<StoreLocationEntity> Locations { get; set; } = new();

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<StoreEntity>, IEntityTypeConfiguration<StoreEntity>
	{
		public override void Configure(EntityTypeBuilder<StoreEntity> config)
		{
			base.Configure(config);

			config.ToTable("Stores");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.Description)
				.HasMaxLength(255);

			config.HasIndex(x => new { x.HouseholdId, x.Title });
		}
	}
}
