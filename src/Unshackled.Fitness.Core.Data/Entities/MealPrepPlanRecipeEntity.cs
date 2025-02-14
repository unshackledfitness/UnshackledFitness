using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Fitness.Core.Data.Entities;

public class MealPrepPlanRecipeEntity : BaseHouseholdEntity
{
	public DateOnly DatePlanned { get; set; }
	public long RecipeId { get; set; }
	public virtual RecipeEntity Recipe { get; set; } = default!;
	public long? SlotId { get; set; }
	public virtual MealPrepPlanSlotEntity? Slot { get; set; }
	public decimal Scale { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<MealPrepPlanRecipeEntity>, IEntityTypeConfiguration<MealPrepPlanRecipeEntity>
	{
		public override void Configure(EntityTypeBuilder<MealPrepPlanRecipeEntity> config)
		{
			base.Configure(config);

			config.ToTable("MealPrepPlanRecipes");

			config.Property(x => x.Scale)
				.HasPrecision(4, 2);

			config.HasOne(x => x.Recipe)
				.WithMany()
				.HasForeignKey(x => x.RecipeId);

			config.HasOne(x => x.Slot)
				.WithMany()
				.HasForeignKey(x => x.SlotId);

			config.HasIndex(x => new { x.DatePlanned, x.SlotId });
		}
	}
}
