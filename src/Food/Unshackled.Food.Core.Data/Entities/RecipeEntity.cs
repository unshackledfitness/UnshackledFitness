using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Food.Core.Data.Entities;

public class RecipeEntity : BaseHouseholdEntity
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int CookTimeMinutes { get; set; }
	public int PrepTimeMinutes { get; set; }
	public int TotalServings { get; set; }

	public virtual List<RecipeIngredientGroupEntity> Groups { get; set; } = [];
	public virtual List<RecipeIngredientEntity> Ingredients { get; set; } = [];
	public virtual List<RecipeStepEntity> Steps { get; set; } = [];
	public virtual List<RecipeNoteEntity> Notes { get; set; } = [];
	public virtual List<TagEntity> Tags { get; set; } = [];

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<RecipeEntity>, IEntityTypeConfiguration<RecipeEntity>
	{
		public override void Configure(EntityTypeBuilder<RecipeEntity> config)
		{
			base.Configure(config);

			config.ToTable("Recipes");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasMany(e => e.Tags)
				.WithMany(e => e.Recipes)
				.UsingEntity<RecipeTagEntity>();

			config.HasIndex(x => new { x.HouseholdId, x.Title });
		}
	}
}
