using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class RecipeStepEntity : BaseHouseholdEntity
{
	public long RecipeId { get; set; }
	public RecipeEntity? Recipe { get; set; }
	public int SortOrder { get; set; }
	public string Instructions { get; set; } = string.Empty;

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<RecipeStepEntity>, IEntityTypeConfiguration<RecipeStepEntity>
	{
		public override void Configure(EntityTypeBuilder<RecipeStepEntity> config)
		{
			base.Configure(config);

			config.ToTable("RecipeSteps");

			config.HasOne(x => x.Recipe)
				.WithMany(x => x.Steps)
				.HasForeignKey(x => x.RecipeId);

			config.HasIndex(x => new { x.RecipeId, x.SortOrder });
		}
	}
}
