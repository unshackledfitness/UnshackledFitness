using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class CookbookRecipeEntity
{
	public long CookbookId { get; set; }
	public virtual CookbookEntity Cookbook { get; set; } = default!;
	public long RecipeId { get; set; }
	public virtual RecipeEntity Recipe { get; set; } = default!;
	public long HouseholdId { get; set; }
	public virtual HouseholdEntity Household { get; set; } = default!;
	public long MemberId { get; set; }
	public virtual MemberEntity Member { get; set; } = default!;

	public class TypeConfiguration : IEntityTypeConfiguration<CookbookRecipeEntity>
	{
		public void Configure(EntityTypeBuilder<CookbookRecipeEntity> config)
		{
			config.ToTable("CookbookRecipes")
				.HasKey(x => new { x.CookbookId, x.RecipeId });

			config.HasOne(x => x.Cookbook)
				.WithMany()
				.HasForeignKey(x => x.CookbookId);

			config.HasOne(x => x.Recipe)
				.WithMany()
				.HasForeignKey(x => x.RecipeId);

			config.HasOne(x => x.Household)
				.WithMany()
				.HasForeignKey(x => x.HouseholdId);

			config.HasOne(x => x.Member)
				.WithMany()
				.HasForeignKey(x => x.MemberId);
		}
	}
}
