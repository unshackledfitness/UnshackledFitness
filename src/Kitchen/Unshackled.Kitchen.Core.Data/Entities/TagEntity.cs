using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class TagEntity : BaseHouseholdEntity
{
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;

	public virtual List<RecipeEntity> Recipes { get; set; } = [];

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<TagEntity>, IEntityTypeConfiguration<TagEntity>
	{
		public override void Configure(EntityTypeBuilder<TagEntity> config)
		{
			base.Configure(config);

			config.ToTable("Tags");

			config.Property(x => x.Key)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => x.Title);
			config.HasIndex(x => new { x.HouseholdId, x.Key })
				.IsUnique();
		}
	}
}
