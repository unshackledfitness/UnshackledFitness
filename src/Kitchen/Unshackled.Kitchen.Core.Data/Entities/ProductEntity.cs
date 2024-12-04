using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Kitchen.Core.Enums;
using Unshackled.Kitchen.Core.Models;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class ProductEntity : BaseHouseholdEntity, INutrition
{
	public string? Brand { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public long? ProductCategoryId { get; set; }
	public virtual ProductCategoryEntity? Category { get; set; }
	public bool IsArchived { get; set; }
	public bool IsPinned { get; set; }
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
	public int CaloriesFromFat { get; set; }
	public decimal TotalFat { get; set; }
	public NutritionUnits TotalFatUnit { get; set; } = NutritionUnits.g;
	public decimal TotalFatN { get; set; }
	public decimal SaturatedFat { get; set; }
	public NutritionUnits SaturatedFatUnit { get; set; } = NutritionUnits.g;
	public decimal SaturatedFatN { get; set; }
	public decimal TransFat { get; set; }
	public NutritionUnits TransFatUnit { get; set; } = NutritionUnits.g;
	public decimal TransFatN { get; set; }
	public decimal PolyunsaturatedFat { get; set; }
	public NutritionUnits PolyunsaturatedFatUnit { get; set; } = NutritionUnits.g;
	public decimal PolyunsaturatedFatN { get; set; }
	public decimal MonounsaturatedFat { get; set; }
	public NutritionUnits MonounsaturatedFatUnit { get; set; } = NutritionUnits.g;
	public decimal MonounsaturatedFatN { get; set; }
	public decimal Cholesterol { get; set; }
	public NutritionUnits CholesterolUnit { get; set; } = NutritionUnits.g;
	public decimal CholesterolN { get; set; }
	public decimal TotalCarbohydrates { get; set; }
	public NutritionUnits TotalCarbohydratesUnit { get; set; } = NutritionUnits.g;
	public decimal TotalCarbohydratesN { get; set; }
	public decimal DietaryFiber { get; set; }
	public NutritionUnits DietaryFiberUnit { get; set; } = NutritionUnits.g;
	public decimal DietaryFiberN { get; set; }
	public decimal SolubleFiber { get; set; }
	public NutritionUnits SolubleFiberUnit { get; set; } = NutritionUnits.g;
	public decimal SolubleFiberN { get; set; }
	public decimal InsolubleFiber { get; set; }
	public NutritionUnits InsolubleFiberUnit { get; set; } = NutritionUnits.g;
	public decimal InsolubleFiberN { get; set; }
	public decimal TotalSugars { get; set; }
	public NutritionUnits TotalSugarsUnit { get; set; } = NutritionUnits.g;
	public decimal TotalSugarsN { get; set; }
	public decimal AddedSugars { get; set; }
	public NutritionUnits AddedSugarsUnit { get; set; } = NutritionUnits.g;
	public decimal AddedSugarsN { get; set; }
	public decimal SugarAlcohols { get; set; }
	public NutritionUnits SugarAlcoholsUnit { get; set; } = NutritionUnits.g;
	public decimal SugarAlcoholsN { get; set; }
	public decimal Protein { get; set; }
	public NutritionUnits ProteinUnit { get; set; } = NutritionUnits.g;
	public decimal ProteinN { get; set; }
	public decimal Biotin { get; set; }
	public NutritionUnits BiotinUnit { get; set; } = NutritionUnits.mcg;
	public decimal BiotinN { get; set; }
	public decimal Choline { get; set; }
	public NutritionUnits CholineUnit { get; set; } = NutritionUnits.mcg;
	public decimal CholineN { get; set; }
	public decimal Folate { get; set; }
	public NutritionUnits FolateUnit { get; set; } = NutritionUnits.mcg;
	public decimal FolateN { get; set; }
	public decimal Niacin { get; set; }
	public NutritionUnits NiacinUnit { get; set; } = NutritionUnits.mcg;
	public decimal NiacinN { get; set; }
	public decimal PantothenicAcid { get; set; }
	public NutritionUnits PantothenicAcidUnit { get; set; } = NutritionUnits.mcg;
	public decimal PantothenicAcidN { get; set; }
	public decimal Riboflavin { get; set; }
	public NutritionUnits RiboflavinUnit { get; set; } = NutritionUnits.mcg;
	public decimal RiboflavinN { get; set; }
	public decimal Thiamin { get; set; }
	public NutritionUnits ThiaminUnit { get; set; } = NutritionUnits.mcg;
	public decimal ThiaminN { get; set; }
	public decimal VitaminA { get; set; }
	public NutritionUnits VitaminAUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminAN { get; set; }
	public decimal VitaminB6 { get; set; }
	public NutritionUnits VitaminB6Unit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminB6N { get; set; }
	public decimal VitaminB12 { get; set; }
	public NutritionUnits VitaminB12Unit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminB12N { get; set; }
	public decimal VitaminC { get; set; }
	public NutritionUnits VitaminCUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminCN { get; set; }
	public decimal VitaminD { get; set; }
	public NutritionUnits VitaminDUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminDN { get; set; }
	public decimal VitaminE { get; set; }
	public NutritionUnits VitaminEUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminEN { get; set; }
	public decimal VitaminK { get; set; }
	public NutritionUnits VitaminKUnit { get; set; } = NutritionUnits.mcg;
	public decimal VitaminKN { get; set; }
	public decimal Calcium { get; set; }
	public NutritionUnits CalciumUnit { get; set; } = NutritionUnits.mg;
	public decimal CalciumN { get; set; }
	public decimal Chloride { get; set; }
	public NutritionUnits ChlorideUnit { get; set; } = NutritionUnits.mg;
	public decimal ChlorideN { get; set; }
	public decimal Chromium { get; set; }
	public NutritionUnits ChromiumUnit { get; set; } = NutritionUnits.mg;
	public decimal ChromiumN { get; set; }
	public decimal Copper { get; set; }
	public NutritionUnits CopperUnit { get; set; } = NutritionUnits.mg;
	public decimal CopperN { get; set; }
	public decimal Iodine { get; set; }
	public NutritionUnits IodineUnit { get; set; } = NutritionUnits.mg;
	public decimal IodineN { get; set; }
	public decimal Iron { get; set; }
	public NutritionUnits IronUnit { get; set; } = NutritionUnits.mg;
	public decimal IronN { get; set; }
	public decimal Magnesium { get; set; }
	public NutritionUnits MagnesiumUnit { get; set; } = NutritionUnits.mg;
	public decimal MagnesiumN { get; set; }
	public decimal Manganese { get; set; }
	public NutritionUnits ManganeseUnit { get; set; } = NutritionUnits.mg;
	public decimal ManganeseN { get; set; }
	public decimal Molybdenum { get; set; }
	public NutritionUnits MolybdenumUnit { get; set; } = NutritionUnits.mg;
	public decimal MolybdenumN { get; set; }
	public decimal Phosphorus { get; set; }
	public NutritionUnits PhosphorusUnit { get; set; } = NutritionUnits.mg;
	public decimal PhosphorusN { get; set; }
	public decimal Potassium { get; set; }
	public NutritionUnits PotassiumUnit { get; set; } = NutritionUnits.mg;
	public decimal PotassiumN { get; set; }
	public decimal Selenium { get; set; }
	public NutritionUnits SeleniumUnit { get; set; } = NutritionUnits.mg;
	public decimal SeleniumN { get; set; }
	public decimal Sodium { get; set; }
	public NutritionUnits SodiumUnit { get; set; } = NutritionUnits.mg;
	public decimal SodiumN { get; set; }
	public decimal Zinc { get; set; }
	public NutritionUnits ZincUnit { get; set; } = NutritionUnits.mg;
	public decimal ZincN { get; set; }

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

			config.Property(x => x.AddedSugars)
				.HasPrecision(7, 2);

			config.Property(x => x.AddedSugarsN)
				.HasPrecision(12, 6);

			config.Property(x => x.Biotin)
				.HasPrecision(7, 2);

			config.Property(x => x.BiotinN)
				.HasPrecision(12, 6);

			config.Property(x => x.Calcium)
				.HasPrecision(7, 2);

			config.Property(x => x.CalciumN)
				.HasPrecision(12, 6);

			config.Property(x => x.Chloride)
				.HasPrecision(7, 2);

			config.Property(x => x.ChlorideN)
				.HasPrecision(12, 6);

			config.Property(x => x.Cholesterol)
				.HasPrecision(7, 2);

			config.Property(x => x.CholesterolN)
				.HasPrecision(12, 6);

			config.Property(x => x.Choline)
				.HasPrecision(7, 2);

			config.Property(x => x.CholineN)
				.HasPrecision(12, 6);

			config.Property(x => x.Chromium)
				.HasPrecision(7, 2);

			config.Property(x => x.ChromiumN)
				.HasPrecision(12, 6);

			config.Property(x => x.Copper)
				.HasPrecision(7, 2);

			config.Property(x => x.CopperN)
				.HasPrecision(12, 6);

			config.Property(x => x.DietaryFiber)
				.HasPrecision(7, 2);

			config.Property(x => x.DietaryFiberN)
				.HasPrecision(12, 6);

			config.Property(x => x.Folate)
				.HasPrecision(7, 2);

			config.Property(x => x.FolateN)
				.HasPrecision(12, 6);

			config.Property(x => x.InsolubleFiber)
				.HasPrecision(7, 2);

			config.Property(x => x.InsolubleFiberN)
				.HasPrecision(12, 6);

			config.Property(x => x.Iodine)
				.HasPrecision(7, 2);

			config.Property(x => x.IodineN)
				.HasPrecision(12, 6);

			config.Property(x => x.Iron)
				.HasPrecision(7, 2);

			config.Property(x => x.IronN)
				.HasPrecision(12, 6);

			config.Property(x => x.Magnesium)
				.HasPrecision(7, 2);

			config.Property(x => x.MagnesiumN)
				.HasPrecision(12, 6);

			config.Property(x => x.Manganese)
				.HasPrecision(7, 2);

			config.Property(x => x.ManganeseN)
				.HasPrecision(12, 6);

			config.Property(x => x.Molybdenum)
				.HasPrecision(7, 2);

			config.Property(x => x.MolybdenumN)
				.HasPrecision(12, 6);

			config.Property(x => x.MonounsaturatedFat)
				.HasPrecision(7, 2);

			config.Property(x => x.MonounsaturatedFatN)
				.HasPrecision(12, 6);

			config.Property(x => x.Niacin)
				.HasPrecision(7, 2);

			config.Property(x => x.NiacinN)
				.HasPrecision(12, 6);

			config.Property(x => x.PantothenicAcid)
				.HasPrecision(7, 2);

			config.Property(x => x.PantothenicAcidN)
				.HasPrecision(12, 6);

			config.Property(x => x.Phosphorus)
				.HasPrecision(7, 2);

			config.Property(x => x.PhosphorusN)
				.HasPrecision(12, 6);

			config.Property(x => x.PolyunsaturatedFat)
				.HasPrecision(7, 2);

			config.Property(x => x.PolyunsaturatedFatN)
				.HasPrecision(12, 6);

			config.Property(x => x.Potassium)
				.HasPrecision(7, 2);

			config.Property(x => x.PotassiumN)
				.HasPrecision(12, 6);

			config.Property(x => x.Protein)
				.HasPrecision(7, 2);

			config.Property(x => x.ProteinN)
				.HasPrecision(12, 6);

			config.Property(x => x.Riboflavin)
				.HasPrecision(7, 2);

			config.Property(x => x.RiboflavinN)
				.HasPrecision(12, 6);

			config.Property(x => x.SaturatedFat)
				.HasPrecision(7, 2);

			config.Property(x => x.SaturatedFatN)
				.HasPrecision(12, 6);

			config.Property(x => x.Selenium)
				.HasPrecision(7, 2);

			config.Property(x => x.SeleniumN)
				.HasPrecision(12, 6);

			config.Property(x => x.Sodium)
				.HasPrecision(7, 2);

			config.Property(x => x.SodiumN)
				.HasPrecision(12, 6);

			config.Property(x => x.SolubleFiber)
				.HasPrecision(7, 2);

			config.Property(x => x.SolubleFiberN)
				.HasPrecision(12, 6);

			config.Property(x => x.SugarAlcohols)
				.HasPrecision(7, 2);

			config.Property(x => x.SugarAlcoholsN)
				.HasPrecision(12, 6);

			config.Property(x => x.TotalSugars)
				.HasPrecision(7, 2);

			config.Property(x => x.TotalSugarsN)
				.HasPrecision(12, 6);

			config.Property(x => x.Thiamin)
				.HasPrecision(7, 2);

			config.Property(x => x.ThiaminN)
				.HasPrecision(12, 6);

			config.Property(x => x.TotalCarbohydrates)
				.HasPrecision(7, 2);

			config.Property(x => x.TotalCarbohydratesN)
				.HasPrecision(12, 6);

			config.Property(x => x.TotalFat)
				.HasPrecision(7, 2);

			config.Property(x => x.TotalFatN)
				.HasPrecision(12, 6);

			config.Property(x => x.TransFat)
				.HasPrecision(7, 2);

			config.Property(x => x.TransFatN)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminA)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminAN)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminB12)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminB12N)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminB6)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminB6N)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminC)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminCN)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminD)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminDN)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminE)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminEN)
				.HasPrecision(12, 6);

			config.Property(x => x.VitaminK)
				.HasPrecision(7, 2);

			config.Property(x => x.VitaminKN)
				.HasPrecision(12, 6);

			config.Property(x => x.Zinc)
				.HasPrecision(7, 2);

			config.Property(x => x.ZincN)
				.HasPrecision(12, 6);

			config.HasOne(x => x.Category)
				.WithMany(x => x.Products)
				.HasForeignKey(x => x.ProductCategoryId);

			config.HasIndex(x => new { x.HouseholdId, x.Title });
		}
	}
}
