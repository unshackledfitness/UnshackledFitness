using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Unshackled.Fitness.Core.Data.Entities;

public abstract class BaseEntity : IDatedEntity
{
	public long Id { get; set; }
	public DateTimeOffset DateCreatedUtc { get; set; }
	public DateTimeOffset? DateLastModifiedUtc { get; set; }
}

public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
	public virtual void Configure(EntityTypeBuilder<TEntity> config)
	{
		config.HasKey(x => x.Id);

		config.HasIndex(x => x.DateCreatedUtc);
		config.HasIndex(x => x.DateLastModifiedUtc);
	}
}
