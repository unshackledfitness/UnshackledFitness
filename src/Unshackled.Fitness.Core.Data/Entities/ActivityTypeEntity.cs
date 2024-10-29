using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Enums;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityTypeEntity : BaseMemberEntity
{
	public string Title { get; set; } = string.Empty;
	public string? Color { get; set; }
	public EventTypes DefaultEventType { get; set; } = EventTypes.Uncategorized;
	public DistanceUnits DefaultDistanceUnits { get; set; }
	public DistanceUnits DefaultElevationUnits { get; set; }
	public SpeedUnits DefaultSpeedUnits { get; set; }
	public CadenceUnits DefaultCadenceUnits { get; set; }

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityTypeEntity>, IEntityTypeConfiguration<ActivityTypeEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityTypeEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivityTypes");

			config.Property(a => a.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.Property(a => a.Color)
				.HasMaxLength(10);

			config.HasIndex(x => new { x.MemberId, x.Title });
		}
	}
}
