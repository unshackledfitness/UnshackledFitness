using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.Core.Data.Entities;

public class ActivityScheduleEntity : BaseMemberEntity
{
	public string Title { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int LengthWeeks { get; set; }
	public DateTime? DateStarted { get; set; }
	public DateTime? DateLastActivityUtc { get; set; }
	public int NextScheduleIndex { get; set; }

	public List<ActivityScheduleItemEntity> Items { get; set; } = new();

	public class TypeConfiguration : BaseMemberEntityTypeConfiguration<ActivityScheduleEntity>, IEntityTypeConfiguration<ActivityScheduleEntity>
	{
		public override void Configure(EntityTypeBuilder<ActivityScheduleEntity> config)
		{
			base.Configure(config);

			config.ToTable("ActivitySchedules");

			config.Property(x => x.Title)
				.HasMaxLength(255)
				.IsRequired();

			config.HasIndex(x => new { x.MemberId, x.Title });
		}
	}
}
