using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class TrainingPlanSessionEntity : BaseMemberEntity
{
	public long TrainingPlanId { get; set; }
	public virtual TrainingPlanEntity Plan { get; set; } = null!;
	public long TrainingSessionId { get; set; }
	public virtual TrainingSessionEntity Session { get; set; } = default!;
	public int WeekNumber { get; set; }
	public int DayNumber { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<TrainingPlanSessionEntity>, IEntityTypeConfiguration<TrainingPlanSessionEntity>
	{
		public override void Configure(EntityTypeBuilder<TrainingPlanSessionEntity> config)
		{
			base.Configure(config);

			config.ToTable("TrainingPlanSessions");

			config.HasOne(x => x.Plan)
				.WithMany(x => x.PlanSessions)
				.HasForeignKey(x => x.TrainingPlanId);

			config.HasOne(x => x.Session)
				.WithMany()
				.HasForeignKey(x => x.TrainingSessionId);

			config.HasIndex(x => new { x.MemberId, x.TrainingPlanId, x.WeekNumber, x.DayNumber, x.SortOrder });
		}
	}
}
