using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class TrainingPlanEntity : BaseMemberEntity
{
	public string Title { get; set; } = string.Empty;
	public ProgramTypes ProgramType { get; set; }
	public DateTime? DateStarted { get; set; }
	public DateTime? DateLastActivityUtc { get; set; }
	public string? Description { get; set; }
	public int LengthWeeks { get; set; }
	public int NextScheduleIndex { get; set; }

	public List<TrainingPlanSessionEntity> PlanSessions { get; set; } = new();

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<TrainingPlanEntity>, IEntityTypeConfiguration<TrainingPlanEntity>
	{
		public override void Configure(EntityTypeBuilder<TrainingPlanEntity> config)
		{
			base.Configure(config);

			config.ToTable("TrainingPlans");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => new { x.MemberId, x.DateStarted });
			config.HasIndex(x => new { x.MemberId, x.Title });
		}
	}
}
