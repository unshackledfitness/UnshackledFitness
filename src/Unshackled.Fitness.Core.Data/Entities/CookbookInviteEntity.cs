using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class CookbookInviteEntity : BaseEntity
{
	public long CookbookId { get; set; }
	public virtual CookbookEntity Cookbook { get; set; } = null!;
	public string Email { get; set; } = string.Empty;
	public PermissionLevels Permissions { get; set; } = PermissionLevels.Read;

	public class TypeConfiguration : BaseEntityTypeConfiguration<CookbookInviteEntity>, IEntityTypeConfiguration<CookbookInviteEntity>
	{
		public override void Configure(EntityTypeBuilder<CookbookInviteEntity> config)
		{
			base.Configure(config);

			config.ToTable("CookbookInvites");

			config.HasOne(x => x.Cookbook)
				.WithMany(x => x.Invites)
				.HasForeignKey(x => x.CookbookId);

			config.Property(x => x.Email)
				 .HasMaxLength(255)
				 .IsRequired();
		}
	}
}
