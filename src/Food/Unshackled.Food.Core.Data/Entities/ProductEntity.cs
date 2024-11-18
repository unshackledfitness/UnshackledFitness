using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Models.Recipes;

namespace Unshackled.Food.Core.Data.Entities;

public class ProductEntity : BaseHouseholdEntity, INutrition
{
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsArchived { get; set; }
	public bool HasNutritionInfo { get; set; }
	public decimal ServingSize { get; set; }
	public decimal ServingSizeN { get; set; }
	public ServingSizeUnits ServingSizeUnit { get; set; } = ServingSizeUnits.Item;
	public string ServingSizeUnitLabel { get; set; } = string.Empty;
	public decimal ServingSizeMetric { get; set; }
	public decimal ServingSizeMetricN { get; set; }
	public ServingSizeMetricUnits ServingSizeMetricUnit { get; set; } = ServingSizeMetricUnits.mg;
	public decimal ServingsPerContainer { get; set; }
	public int Calories { get; set; }
	public decimal Carbohydrates { get; set; }
	public NutritionUnits CarbohydratesUnit { get; set; } = NutritionUnits.g;
	public decimal CarbohydratesN { get; set; }
	public decimal Fat { get; set; }
	public NutritionUnits FatUnit { get; set; } = NutritionUnits.g;
	public decimal FatN { get; set; }
	public decimal Protein { get; set; }
	public NutritionUnits ProteinUnit { get; set; } = NutritionUnits.g;
	public decimal ProteinN { get; set; }

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<ProductEntity>, IEntityTypeConfiguration<ProductEntity>
	{
		public override void Configure(EntityTypeBuilder<ProductEntity> config)
		{
			base.Configure(config);

			config.ToTable("Products");

			config.Property(x => x.Brand)
				.HasMaxLength(255);

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.Description)
				.HasMaxLength(255);

			config.Property(x => x.ServingSize)
				.HasPrecision(8, 3);

			config.Property(x => x.ServingSizeN)
				.HasPrecision(13, 6);

			config.Property(x => x.ServingSizeMetric)
				.HasPrecision(7, 2);

			config.Property(x => x.ServingSizeMetricN)
				.HasPrecision(12, 6);

			config.Property(x => x.ServingSizeUnitLabel)
				.HasMaxLength(25)
				.IsRequired();

			config.Property(x => x.ServingsPerContainer)
				.HasPrecision(8, 3);

			config.Property(x => x.Carbohydrates)
				.HasPrecision(7, 2);

			config.Property(x => x.CarbohydratesN)
				.HasPrecision(12, 6);

			config.Property(x => x.Fat)
				.HasPrecision(7, 2);

			config.Property(x => x.FatN)
				.HasPrecision(12, 6);

			config.Property(x => x.Protein)
				.HasPrecision(7, 2);

			config.Property(x => x.ProteinN)
				.HasPrecision(12, 6);

			config.HasIndex(x => new { x.HouseholdId, x.Title });
		}
	}
}
