using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class MealPlanRecipeEntity : BaseHouseholdEntity
{
	public DateOnly DatePlanned { get; set; }
	public long RecipeId { get; set; }
	public virtual RecipeEntity Recipe { get; set; } = default!;
	public long? MealDefinitionId { get; set; }
	public virtual MealDefinitionEntity? MealDefinition { get; set; }
	public decimal Scale { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<MealPlanRecipeEntity>, IEntityTypeConfiguration<MealPlanRecipeEntity>
	{
		public override void Configure(EntityTypeBuilder<MealPlanRecipeEntity> config)
		{
			base.Configure(config);

			config.ToTable("MealPlanRecipes");

			config.Property(x => x.Scale)
				.HasPrecision(4, 2);

			config.HasOne(x => x.Recipe)
				.WithMany()
				.HasForeignKey(x => x.RecipeId);

			config.HasOne(x => x.MealDefinition)
				.WithMany()
				.HasForeignKey(x => x.MealDefinitionId);

			config.HasIndex(x => new { x.DatePlanned, x.MealDefinitionId });
		}
	}
}
