using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Kitchen.Core.Data.Entities;

public abstract class BaseHouseholdEntity : BaseEntity
{
	public long HouseholdId { get; set; }
	public virtual HouseholdEntity Household { get; set; } = null!;
}

public abstract class BaseHouseholdEntityTypeConfiguration<TEntity> : BaseEntityTypeConfiguration<TEntity>, IEntityTypeConfiguration<TEntity> where TEntity : BaseHouseholdEntity
{
	public override void Configure(EntityTypeBuilder<TEntity> config)
	{
		base.Configure(config);

		config.HasOne(p => p.Household)
			.WithMany()
			.HasForeignKey(p => p.HouseholdId);

		config.HasIndex(p => p.HouseholdId);
	}

	public void Configure(EntityTypeBuilder<TEntity> config, Expression<Func<HouseholdEntity, IEnumerable<TEntity>?>> navigationExpression)
	{
		base.Configure(config);

		config.HasOne(p => p.Household)
			.WithMany(navigationExpression)
			.HasForeignKey(p => p.HouseholdId);
	}
}
