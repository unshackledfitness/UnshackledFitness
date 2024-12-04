using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Kitchen.Core.Data.Entities;

public class CookbookEntity : BaseMemberEntity
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }

	public List<CookbookMemberEntity> Memberships { get; set; } = [];
	public List<CookbookInviteEntity> Invites { get; set; } = [];

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<CookbookEntity>, IEntityTypeConfiguration<CookbookEntity>
	{
		public override void Configure(EntityTypeBuilder<CookbookEntity> config)
		{
			base.Configure(config);

			config.ToTable("Cookbooks");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(x => x.Description)
				.HasMaxLength(500);
		}
	}
}
