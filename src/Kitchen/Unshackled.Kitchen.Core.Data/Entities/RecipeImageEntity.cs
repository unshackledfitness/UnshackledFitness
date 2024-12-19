using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class RecipeImageEntity : BaseHouseholdEntity
{
	public long RecipeId { get; set; }
	public virtual RecipeEntity Recipe { get; set; } = default!;
	public string Container { get; set; } = string.Empty;
	public string RelativePath { get; set; } = string.Empty;
	public string MimeType { get; set; } = string.Empty;
	public long FileSize { get; set; }
	public bool IsFeatured { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<RecipeImageEntity>, IEntityTypeConfiguration<RecipeImageEntity>
	{
		public override void Configure(EntityTypeBuilder<RecipeImageEntity> config)
		{
			base.Configure(config);

			config.ToTable("RecipeImages");

			config.Property(x => x.Container)
				.HasMaxLength(50)
				.IsRequired();

			config.Property(x => x.RelativePath)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.MimeType)
				.HasMaxLength(50)
				.IsRequired();

			config.HasOne(x => x.Recipe)
				.WithMany(x => x.Images)
				.HasForeignKey(x => x.RecipeId);

			config.HasIndex(x => x.RecipeId);
		}
	}
}
