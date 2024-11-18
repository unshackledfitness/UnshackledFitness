using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class StoreLocationEntity : BaseHouseholdEntity
{
	public long StoreId { get; set; }
	public virtual StoreEntity Store { get; set; } = default!;
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<StoreLocationEntity>, IEntityTypeConfiguration<StoreLocationEntity>
	{
		public override void Configure(EntityTypeBuilder<StoreLocationEntity> config)
		{
			base.Configure(config);

			config.ToTable("StoreLocations");

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.Description)
				.HasMaxLength(255);

			config.HasOne(x => x.Store)
				.WithMany(x => x.Locations)
				.HasForeignKey(x => x.StoreId);

			config.HasIndex(x => new { x.HouseholdId, x.StoreId, x.SortOrder });
		}
	}
}
