using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Fitness.Core.Data.Entities;

public class RecipeNoteEntity : BaseHouseholdEntity
{
	public long RecipeId { get; set; }
	public RecipeEntity? Recipe { get; set; }
	public int SortOrder { get; set; }
	public string Note { get; set; } = string.Empty;

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<RecipeNoteEntity>, IEntityTypeConfiguration<RecipeNoteEntity>
	{
		public override void Configure(EntityTypeBuilder<RecipeNoteEntity> config)
		{
			base.Configure(config);

			config.ToTable("RecipeNotes");

			config.HasOne(x => x.Recipe)
				.WithMany(x => x.Notes)
				.HasForeignKey(x => x.RecipeId);

			config.HasIndex(x => new { x.RecipeId, x.SortOrder });
		}
	}
}
