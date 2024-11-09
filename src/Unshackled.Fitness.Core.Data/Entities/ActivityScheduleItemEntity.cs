using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityScheduleItemEntity : BaseMemberEntity
{
	public long ScheduleId { get; set; }
	public virtual ActivityScheduleEntity Schedule { get; set; } = null!;
	public long ActivityTypeId { get; set; }
	public virtual ActivityTypeEntity ActivityType { get; set; } = default!;
	public string Title { get; set; } = string.Empty;
	public EventTypes DefaultEventType {  get; set; }
	public int? TargetTimeSeconds { get; set; }
	public double? TargetDistanceMeters { get; set; }
	public int? TargetCalories { get; set; }
	public int? TargetPace { get; set; }
	public int? TargetHeartRateBpm { get; set; }
	public double? TargetCadence { get; set; }
	public double? TargetPower { get; set; }
	public string? Notes { get; set; }
	public int WeekNumber { get; set; }
	public int DayNumber { get; set; }
	public int SortOrder { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityScheduleItemEntity>, IEntityTypeConfiguration<ActivityScheduleItemEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityScheduleItemEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivitieScheduleItems");

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasOne(x => x.Schedule)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.ScheduleId);

			config.HasOne(x => x.ActivityType)
				.WithMany()
				.HasForeignKey(x => x.ActivityTypeId);

			config.HasIndex(x => new { x.MemberId, x.ScheduleId, x.WeekNumber, x.DayNumber, x.SortOrder });
		}
	}
}
