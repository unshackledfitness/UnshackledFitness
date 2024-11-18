using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Food.Core.Enums;

namespace Unshackled.Food.Core.Data.Entities;

public class RecipeIngredientEntity : BaseHouseholdEntity
{
	public long RecipeId { get; set; }
	public RecipeEntity? Recipe { get; set; }
	public string Key { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public long ListGroupId { get; set; }
	public virtual RecipeIngredientGroupEntity? ListGroup { get; set; }
	public int SortOrder { get; set; }
	public decimal Amount { get; set; }
	public decimal AmountN { get; set; } // Amount in mg or ml
	public MeasurementUnits AmountUnit { get; set; } = MeasurementUnits.mg;
	public string AmountLabel { get; set; } = string.Empty;
	public string AmountText { get; set; } = string.Empty;
	public string? PrepNote { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<RecipeIngredientEntity>, IEntityTypeConfiguration<RecipeIngredientEntity>
	{
		public override void Configure(EntityTypeBuilder<RecipeIngredientEntity> config)
		{
			base.Configure(config);

			config.ToTable("RecipeIngredients");

			config.Property(x => x.Key)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.Amount)
				.HasPrecision(8, 3);

			config.Property(x => x.AmountN)
				.HasPrecision(15, 3);

			config.Property(x => x.AmountLabel)
				.HasMaxLength(25)
				.IsRequired();

			config.Property(x => x.AmountText)
				.HasMaxLength(15)
				.IsRequired();

			config.Property(x => x.PrepNote)
				.HasMaxLength(255);

			config.HasOne(x => x.ListGroup)
				.WithMany(x => x.Ingredients)
				.HasForeignKey(x => x.ListGroupId);

			config.HasOne(x => x.Recipe)
				.WithMany(x => x.Ingredients)
				.HasForeignKey(x => x.RecipeId);

			config.HasIndex(x => new { x.RecipeId, x.SortOrder });
			config.HasIndex(x => new { x.HouseholdId, x.Key });
		}
	}
}
