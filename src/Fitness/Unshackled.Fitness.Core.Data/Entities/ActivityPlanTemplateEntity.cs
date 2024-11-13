using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityPlanTemplateEntity : BaseMemberEntity
{
	public long PlanId { get; set; }
	public virtual ActivityPlanEntity Plan { get; set; } = null!;
	public long ActivityTargetId { get; set; }
	public virtual ActivityTemplateEntity Target { get; set; } = default!;
	public int WeekNumber { get; set; }
	public int DayNumber { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityPlanTemplateEntity>, IEntityTypeConfiguration<ActivityPlanTemplateEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityPlanTemplateEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivityPlanTemplates");

			config.HasOne(x => x.Plan)
				.WithMany(x => x.Templates)
				.HasForeignKey(x => x.PlanId);

			config.HasOne(x => x.Target)
				.WithMany()
				.HasForeignKey(x => x.ActivityTargetId);

			config.HasIndex(x => new { x.MemberId, x.PlanId, x.WeekNumber, x.DayNumber, x.SortOrder });
		}
	}
}
