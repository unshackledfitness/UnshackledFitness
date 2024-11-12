using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityTargetEntity : BaseMemberEntity
{
	public long ActivityTypeId { get; set; }
	public virtual ActivityTypeEntity ActivityType { get; set; } = default!;
	public string Title { get; set; } = string.Empty;
	public EventTypes EventType {  get; set; }
	public int? TargetTimeSeconds { get; set; }
	public double? TargetDistanceMeters { get; set; }
	public int? TargetCalories { get; set; }
	public int? TargetPace { get; set; }
	public int? TargetHeartRateBpm { get; set; }
	public double? TargetCadence { get; set; }
	public double? TargetPower { get; set; }
	public string? Notes { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityTargetEntity>, IEntityTypeConfiguration<ActivityTargetEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityTargetEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivityTargets");

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
