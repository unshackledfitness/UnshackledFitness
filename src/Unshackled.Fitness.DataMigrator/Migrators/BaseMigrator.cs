using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Unshackled.Studio.Core.Data;
using Unshackled.Studio.Core.Data.Entities;
using Unshackled.Studio.DataMigrator.Extensions;

namespace Unshackled.Studio.DataMigrator.Migrators;
internal abstract class BaseMigrator<TContext> where TContext : BaseDbContext
{
	protected const int pageSize = 50;

	public TContext dbLegacy = default!;
	public TContext dbNew = default!;
	protected bool isIdentityInsertRequired;

	protected async Task MigrateDbSet<TEntity>(DbSet<TEntity> dbSource, DbSet<TEntity> dbDest, string title)
		where TEntity : BaseEntity
	{
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbSource.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbSource
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbDest.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<TEntity>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateMemberMeta()
	{
		string title = "Member Meta";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.MemberMeta.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.MemberMeta
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.MemberMeta.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<MemberMetaEntity>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateRoleClaims()
	{
		string title = "Role Claims";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.RoleClaims.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.RoleClaims
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.RoleClaims.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityRoleClaim<string>>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateRoles()
	{
		string title = "Roles";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.Roles.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.Roles
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.Roles.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityRole>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserRoles()
	{
		string title = "User Roles";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserRoles.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserRoles
					.AsNoTracking()
					.OrderBy(x => x.UserId)
						.ThenBy(x => x.RoleId)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserRoles.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserClaims()
	{
		string title = "User Claims";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserClaims.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserClaims
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserClaims.AddRange(list);
					if (isIdentityInsertRequired)
						await dbNew.SaveChangesWithIdentityInsert<IdentityUserClaim<string>>();
					else
						await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserLogins()
	{
		string title = "Users";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserLogins.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserLogins
					.AsNoTracking()
					.OrderBy(x => x.LoginProvider)
						.ThenBy(x => x.ProviderKey)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserLogins.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUsers()
	{
		string title = "Users";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.Users.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.Users
					.AsNoTracking()
					.OrderBy(x => x.Id)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.Users.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected async Task MigrateUserTokens()
	{
		string title = "User Tokens";
		Msg.WriteHeader(title);
		Msg.WriteOngoing($"Migrating {title}");
		int count = await dbLegacy.UserTokens.CountAsync();
		if (count > 0)
		{
			Msg.WriteOngoing($"found {count}");

			int pages = (int)Math.Ceiling((decimal)count / pageSize);
			for (int i = 0; i < pages; i++)
			{
				var list = await dbLegacy.UserTokens
					.AsNoTracking()
					.OrderBy(x => x.UserId)
						.ThenBy(x => x.LoginProvider)
							.ThenBy(x => x.Name)
					.Skip(i * pageSize)
					.Take(pageSize)
					.ToListAsync();

				if (list.Count > 0)
				{
					dbNew.UserTokens.AddRange(list);
					await dbNew.SaveChangesAsync();
					Msg.WriteDot();
				}
			}
			Msg.WriteComplete();
		}
		else
		{
			Msg.WriteComplete($"found {count}");
		}
	}

	protected void PrepareSqlitePath(string connString)
	{
		Msg.WriteOngoing("Preparing directories");

		string path = connString
			.Replace("Data Source=", string.Empty);

		if (Path.DirectorySeparatorChar != '/')
		{
			path = path.Replace('/', Path.DirectorySeparatorChar);
		}

		int idxLastSlash = path.LastIndexOf(Path.DirectorySeparatorChar);
		string dirPath = path.Substring(0, idxLastSlash + 1);

		Directory.CreateDirectory(dirPath);

		if (File.Exists(path))
		{
			File.Delete(path);
		}
		Msg.WriteComplete();
	}
}
