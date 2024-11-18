using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class RecipeStepIngredientEntity
{
	public long RecipeStepId { get; set; }
	public RecipeStepEntity? RecipeStep { get; set; }
	public long RecipeIngredientId { get; set; }
	public RecipeIngredientEntity? RecipeIngredient { get; set; }
	public long RecipeId { get; set; }
	public RecipeEntity? Recipe { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<RecipeStepIngredientEntity>
	{
		public void Configure(EntityTypeBuilder<RecipeStepIngredientEntity> config)
		{
			config.ToTable("RecipeStepIngredients")
				.HasKey(x => new { x.RecipeStepId, x.RecipeIngredientId });

			config.HasOne(x => x.RecipeStep)
				.WithMany(x => x.Ingredients)
				.HasForeignKey(x => x.RecipeStepId);

			config.HasOne(x => x.RecipeIngredient)
				.WithMany()
				.HasForeignKey(x => x.RecipeIngredientId);

			config.HasOne(x => x.Recipe)
				.WithMany()
				.HasForeignKey(x => x.RecipeId);

			config.HasIndex(x => x.RecipeStepId);
		}
	}
}
