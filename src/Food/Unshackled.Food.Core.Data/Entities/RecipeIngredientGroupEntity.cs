using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class RecipeIngredientGroupEntity : BaseHouseholdEntity
{
	public long RecipeId { get; set; }
	public RecipeEntity? Recipe { get; set; }
	public int SortOrder { get; set; }
	public string Title { get; set; } = string.Empty;

	public virtual List<RecipeIngredientEntity> Ingredients { get; set; } = new();

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<RecipeIngredientGroupEntity>, IEntityTypeConfiguration<RecipeIngredientGroupEntity>
	{
		public override void Configure(EntityTypeBuilder<RecipeIngredientGroupEntity> config)
		{
			base.Configure(config);

			config.ToTable("RecipeIngredientGroups");

			config.HasOne(x => x.Recipe)
				.WithMany(x => x.Groups)
				.HasForeignKey(x => x.RecipeId);

			config.HasIndex(x => new { x.RecipeId, x.SortOrder });
		}
	}
}
