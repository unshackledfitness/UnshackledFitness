using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityPlanTargetEntity : BaseMemberEntity
{
	public long PlanId { get; set; }
	public virtual ActivityPlanEntity Plan { get; set; } = null!;
	public long ActivityTargetId { get; set; }
	public virtual ActivityTargetEntity Target { get; set; } = default!;
	public int WeekNumber { get; set; }
	public int DayNumber { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityPlanTargetEntity>, IEntityTypeConfiguration<ActivityPlanTargetEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityPlanTargetEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivityPlanTargets");

			config.HasOne(x => x.Plan)
				.WithMany(x => x.Targets)
				.HasForeignKey(x => x.PlanId);

			config.HasOne(x => x.Target)
				.WithMany()
				.HasForeignKey(x => x.ActivityTargetId);

			config.HasIndex(x => new { x.MemberId, x.PlanId, x.WeekNumber, x.DayNumber, x.SortOrder });
		}
	}
}
