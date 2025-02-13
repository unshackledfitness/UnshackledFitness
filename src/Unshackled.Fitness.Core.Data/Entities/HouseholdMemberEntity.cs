using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class HouseholdMemberEntity
{
	public long HouseholdId { get; set; }
	public virtual HouseholdEntity Household { get; set; } = null!;
	public long MemberId { get; set; }
	public virtual MemberEntity Member { get; set; } = null!;
	public PermissionLevels PermissionLevel { get; set; }

	public class TypeConfiguration : IEntityTypeConfiguration<HouseholdMemberEntity>
	{
		public void Configure(EntityTypeBuilder<HouseholdMemberEntity> config)
		{
			config.ToTable("HouseholdMembers");
			config.HasKey(x => new { x.HouseholdId, x.MemberId });

			config.HasOne(x => x.Household)
				.WithMany(x => x.Memberships)
				.HasForeignKey(x => x.HouseholdId);

			config.HasOne(x => x.Member)
				.WithMany()
				.HasForeignKey(x => x.MemberId);
		}
	}
}
