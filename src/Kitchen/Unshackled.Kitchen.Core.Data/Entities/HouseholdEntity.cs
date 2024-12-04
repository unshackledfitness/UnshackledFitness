using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class HouseholdEntity : BaseMemberEntity
{
	public string Title { get; set; } = string.Empty;

	public List<HouseholdMemberEntity> Memberships { get; set; } = new();
	public List<HouseholdInviteEntity> Invites { get; set; } = new();

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<HouseholdEntity>, IEntityTypeConfiguration<HouseholdEntity>
	{
		public override void Configure(EntityTypeBuilder<HouseholdEntity> config)
		{
			base.Configure(config);

			config.ToTable("Households");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();
		}
	}
}
