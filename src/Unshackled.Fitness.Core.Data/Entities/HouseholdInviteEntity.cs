using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Data.Entities;

public class HouseholdInviteEntity : BaseHouseholdEntity
{
	public string Email { get; set; } = string.Empty;
	public PermissionLevels Permissions { get; set; } = PermissionLevels.Read;

	public class TypeConfiguration : BaseHouseholdEntityTypeConfiguration<HouseholdInviteEntity>, IEntityTypeConfiguration<HouseholdInviteEntity>
	{
		public override void Configure(EntityTypeBuilder<HouseholdInviteEntity> config)
		{
			base.Configure(config, x => x.Invites);

			config.ToTable("HouseholdInvites");

			config.Property(x => x.Email)
				 .HasMaxLength(255)
				 .IsRequired();
		}
	}
}
