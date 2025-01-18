using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class MealDefinitionEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<MealDefinitionEntity>, IEntityTypeConfiguration<MealDefinitionEntity>
	{
		public override void Configure(EntityTypeBuilder<MealDefinitionEntity> config)
		{
			base.Configure(config);

			config.ToTable("MealDefinitions");

			config.Property(a => a.Title)
				.HasMaxLength(50)
				.IsRequired();

			config.HasIndex(x => x.Title);
			config.HasIndex(x => x.SortOrder);
		}
	}
}
