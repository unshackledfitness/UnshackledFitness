using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Fitness.Core.Data.Entities;

public class MealPrepPlanSlotEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<MealPrepPlanSlotEntity>, IEntityTypeConfiguration<MealPrepPlanSlotEntity>
	{
		public override void Configure(EntityTypeBuilder<MealPrepPlanSlotEntity> config)
		{
			base.Configure(config);

			config.ToTable("MealPrepPlanSlots");

			config.Property(a => a.Title)
				.HasMaxLength(50)
				.IsRequired();

			config.HasIndex(x => x.Title);
			config.HasIndex(x => x.SortOrder);
		}
	}
}
