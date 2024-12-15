using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Studio.Core.Data;

public class BaseDbContext : IdentityDbContext<UserEntity>
{
	protected readonly ConnectionStrings ConnectionStrings;
	protected readonly DbConfiguration DbConfig;

	public BaseDbContext(DbContextOptions<BaseDbContext> options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options)
	{
		this.ConnectionStrings = connectionStrings;
		this.DbConfig = dbConfig;
	}

	protected BaseDbContext(DbContextOptions options,
		ConnectionStrings connectionStrings,
		DbConfiguration dbConfig) : base(options)
	{
		this.ConnectionStrings = connectionStrings;
		this.DbConfig = dbConfig;
	}

	public DbSet<MemberMetaEntity> MemberMeta => Set<MemberMetaEntity>();
	public DbSet<MemberEntity> Members => Set<MemberEntity>();

	private static void ApplyDatedDefaults(EntityEntry<IDatedEntity> entry)
	{
		switch (entry.State)
		{
			case EntityState.Added:
				entry.Entity.DateCreatedUtc = DateTime.UtcNow;
				break;
			case EntityState.Modified:
				entry.Entity.DateLastModifiedUtc = DateTime.UtcNow;
				break;
		}
	}

	public override int SaveChanges()
	{
		foreach (var entry in ChangeTracker.Entries<IDatedEntity>())
		{
			ApplyDatedDefaults(entry);
		}

		return base.SaveChanges();
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		foreach (var entry in ChangeTracker.Entries<IDatedEntity>())
		{
			ApplyDatedDefaults(entry);
		}

		return base.SaveChangesAsync(cancellationToken);
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfiguration(new MemberMetaEntity.TypeConfiguration());
		builder.ApplyConfiguration(new MemberEntity.TypeConfiguration());

		builder.Entity<UserEntity>(x =>
		{
			x.ToTable("Users");
		});

		builder.Entity<IdentityUserClaim<string>>(x =>
		{
			x.ToTable("UserClaims");
		});

		builder.Entity<IdentityUserLogin<string>>(x =>
		{
			x.ToTable("UserLogins");
		});

		builder.Entity<IdentityUserToken<string>>(x =>
		{
			x.ToTable("UserTokens");
		});

		builder.Entity<IdentityRole>(x =>
		{
			x.ToTable("Roles");
		});

		builder.Entity<IdentityRoleClaim<string>>(x =>
		{
			x.ToTable("RoleClaims");
		});

		builder.Entity<IdentityUserRole<string>>(x =>
		{
			x.ToTable("UserRoles");
		});

		if (!string.IsNullOrEmpty(DbConfig.TablePrefix))
		{
			foreach (var entity in builder.Model.GetEntityTypes())
			{
				entity.SetTableName(DbConfig.TablePrefix + entity.GetTableName());
			}
		}

		foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
		{
			relationship.DeleteBehavior = DeleteBehavior.NoAction;
		}
	}
}
