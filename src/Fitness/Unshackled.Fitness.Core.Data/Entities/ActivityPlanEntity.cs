using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityPlanEntity : BaseMemberEntity
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int LengthWeeks { get; set; }
	public DateTime? DateStarted { get; set; }
	public DateTime? DateLastActivityUtc { get; set; }
	public int NextScheduleIndex { get; set; }

	public List<ActivityPlanTemplateEntity> Templates { get; set; } = new();

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityPlanEntity>, IEntityTypeConfiguration<ActivityPlanEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityPlanEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivityPlans");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => new { x.MemberId, x.DateStarted });
			config.HasIndex(x => new { x.MemberId, x.Title });
		}
	}
}
