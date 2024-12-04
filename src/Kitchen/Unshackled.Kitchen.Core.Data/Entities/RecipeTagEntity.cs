using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class RecipeTagEntity
{
	public long RecipeId { get; set; }
	public RecipeEntity Recipe { get; set; } = default!;
	public long TagId { get; set; }
	public TagEntity Tag { get; set; } = default!;

	public class TypeConfiguration : IEntityTypeConfiguration<RecipeTagEntity>
	{
		public void Configure(EntityTypeBuilder<RecipeTagEntity> config)
		{
			config.ToTable("RecipeTags")
				.HasKey(x => new { x.RecipeId, x.TagId });
		}
	}
}
