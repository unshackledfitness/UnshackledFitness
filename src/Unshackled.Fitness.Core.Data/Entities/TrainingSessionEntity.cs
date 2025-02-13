using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class TrainingSessionEntity : BaseMemberEntity
{
	public long ActivityTypeId { get; set; }
	public virtual ActivityTypeEntity ActivityType { get; set; } = default!;
	public string Title { get; set; } = string.Empty;
	public EventTypes EventType {  get; set; }
	public double? TargetCadence { get; set; }
	public CadenceUnits TargetCadenceUnit { get; set; }
	public int? TargetCalories { get; set; }
	public double? TargetDistance { get; set; }
	public double? TargetDistanceN { get; set; }
	public DistanceUnits TargetDistanceUnit { get; set; }
	public int? TargetHeartRateBpm { get; set; }
	public int? TargetPace { get; set; }
	public double? TargetPower { get; set; }
	public int? TargetTimeSeconds { get; set; }
	public string? Notes { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<TrainingSessionEntity>, IEntityTypeConfiguration<TrainingSessionEntity>
	{
		public override void Configure(EntityTypeBuilder<TrainingSessionEntity> config)
		{
			base.Configure(config);

			config.ToTable("TrainingSessions");

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasOne(x => x.ActivityType)
				.WithMany()
				.HasForeignKey(x => x.ActivityTypeId);

			config.HasIndex(x => new { x.MemberId, x.ActivityTypeId, x.Title });
			config.HasIndex(x => new { x.MemberId, x.Title });
		}
	}
}
