using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Utils;
using Unshackled.Studio.Core.Data.Entities;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Extensions;

public static class MemberExtensions
{
	public static async Task<MemberEntity?> AddMember(this FitnessDbContext db, string email, SiteConfiguration siteConfig)
	{
		email = email.Trim().ToLower();

		if (string.IsNullOrEmpty(email))
			throw new NullReferenceException("Email cannot be empty.");

		var member = await db.Members
			.Where(m => m.Email == email)
			.SingleOrDefaultAsync();

		if (member != null)
			return member;

		using var transaction = await db.Database.BeginTransactionAsync();

		try
		{
			// Create member
			member = new MemberEntity
			{
				Email = email,
				IsActive = true,
			};
			db.Members.Add(member);
			await db.SaveChangesAsync();

			await AddSampleData(db, member.Id);

			await transaction.CommitAsync();
			return member;
		}
		catch
		{
			await transaction.RollbackAsync();
			return null;
		}
	}

	public static async Task<Member> GetMember(this FitnessDbContext db, MemberEntity memberEntity)
	{
		var member = new Member
		{
			AppTheme = memberEntity.AppTheme,
			DateCreatedUtc = memberEntity.DateCreatedUtc,
			DateLastModifiedUtc = memberEntity.DateLastModifiedUtc,
			Email = memberEntity.Email,
			Sid = memberEntity.Id.Encode(),
			IsActive = memberEntity.IsActive,
		};

		member.Settings = await db.GetMemberSettings(memberEntity.Id);

		return member;
	}

	public static async Task<AppSettings> GetMemberSettings(this FitnessDbContext db, long memberId)
	{
		string? settingsJson = await db.MemberMeta.GetMeta(memberId, FitnessGlobals.MetaKeys.AppSettings);

		if (!string.IsNullOrEmpty(settingsJson))
		{
			return JsonSerializer.Deserialize<AppSettings>(settingsJson) ?? new();
		}

		return new();
	}

	public static async Task SaveMemberSettings(this FitnessDbContext db, long memberId, AppSettings settings)
	{
		string settingsJson = JsonSerializer.Serialize(settings);
		await db.SaveMeta(memberId, FitnessGlobals.MetaKeys.AppSettings, settingsJson);
	}

	private static async Task AddSampleData(FitnessDbContext db, long memberId)
	{
		// **************
		// Add exercises
		// **************
		var exBenchPress = new ExerciseEntity
		{
			MemberId = memberId,
			DefaultSetType = WorkoutSetTypes.Standard,
			DefaultSetMetricType = SetMetricTypes.WeightReps,
			Equipment = new EquipmentTypes[] { EquipmentTypes.Barbell }.ToJoinedIntString(),
			Muscles = new MuscleTypes[] { MuscleTypes.Pectorals, MuscleTypes.Triceps }.ToJoinedIntString(),
			IsTrackingSplit = false,
			Title = "BENCH PRESS"
		};

		var exSquat = new ExerciseEntity
		{
			MemberId = memberId,
			DefaultSetType = WorkoutSetTypes.Standard,
			DefaultSetMetricType = SetMetricTypes.WeightReps,
			Equipment = new EquipmentTypes[] { EquipmentTypes.Barbell }.ToJoinedIntString(),
			Muscles = new MuscleTypes[] { MuscleTypes.Quadriceps, MuscleTypes.Glutes }.ToJoinedIntString(),
			IsTrackingSplit = false,
			Title = "SQUAT"
		};

		var exRow = new ExerciseEntity
		{
			MemberId = memberId,
			DefaultSetType = WorkoutSetTypes.Standard,
			DefaultSetMetricType = SetMetricTypes.WeightReps,
			Equipment = new EquipmentTypes[] { EquipmentTypes.Cable, EquipmentTypes.TriangleRow }.ToJoinedIntString(),
			Muscles = new MuscleTypes[] { MuscleTypes.MiddleBack }.ToJoinedIntString(),
			IsTrackingSplit = false,
			Title = "ROW: SEATED"
		};

		var exDeadLift = new ExerciseEntity
		{
			MemberId = memberId,
			DefaultSetType = WorkoutSetTypes.Standard,
			DefaultSetMetricType = SetMetricTypes.WeightReps,
			Equipment = new EquipmentTypes[] { EquipmentTypes.Barbell }.ToJoinedIntString(),
			Muscles = new MuscleTypes[] { MuscleTypes.LowerBack, MuscleTypes.Glutes, MuscleTypes.Hamstrings }.ToJoinedIntString(),
			IsTrackingSplit = false,
			Title = "DEADLIFT"
		};

		var exLatPullDown = new ExerciseEntity
		{
			MemberId = memberId,
			DefaultSetType = WorkoutSetTypes.Standard,
			DefaultSetMetricType = SetMetricTypes.WeightReps,
			Equipment = new EquipmentTypes[] { EquipmentTypes.Cable, EquipmentTypes.LatBar }.ToJoinedIntString(),
			Muscles = new MuscleTypes[] { MuscleTypes.Lats, MuscleTypes.Biceps }.ToJoinedIntString(),
			IsTrackingSplit = false,
			Title = "LAT PULL-DOWN"
		};

		var exShoulderPress = new ExerciseEntity
		{
			MemberId = memberId,
			DefaultSetType = WorkoutSetTypes.Standard,
			DefaultSetMetricType = SetMetricTypes.WeightReps,
			Equipment = new EquipmentTypes[] { EquipmentTypes.Barbell }.ToJoinedIntString(),
			Muscles = new MuscleTypes[] { MuscleTypes.Deltoids }.ToJoinedIntString(),
			IsTrackingSplit = false,
			Title = "SHOULDER PRESS"
		};

		db.Exercises.AddRange(exBenchPress, exSquat, exRow, exDeadLift, exLatPullDown, exShoulderPress);
		await db.SaveChangesAsync();

		// **************
		// Add templates
		// **************

		var tmpAMuscles = new List<MuscleTypes>();
		tmpAMuscles.AddRange(EnumUtils.FromJoinedIntString<MuscleTypes>(exBenchPress.Muscles));
		tmpAMuscles.AddRange(EnumUtils.FromJoinedIntString<MuscleTypes>(exSquat.Muscles));
		tmpAMuscles.AddRange(EnumUtils.FromJoinedIntString<MuscleTypes>(exRow.Muscles));

		var tmpA = new WorkoutTemplateEntity
		{
			ExerciseCount = 3,
			MemberId = memberId,
			MusclesTargeted = tmpAMuscles.Titles(),
			SetCount = 12,
			Title = "Sample Workout A"
		};
		db.WorkoutTemplates.Add(tmpA);
		await db.SaveChangesAsync();

		var groupA = new WorkoutTemplateSetGroupEntity()
		{
			WorkoutTemplateId = tmpA.Id,
			MemberId = memberId,
			SortOrder = 0,
			Title = string.Empty
		};
		db.WorkoutTemplateSetGroups.Add(groupA);
		await db.SaveChangesAsync();

		db.WorkoutTemplateSets.AddRange(
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exSquat.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 20,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Warmup,
				SortOrder = 0,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exSquat.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 1,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exSquat.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 2,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exSquat.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 3,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exBenchPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 20,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Warmup,
				SortOrder = 4,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exBenchPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 5,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exBenchPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 6,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exBenchPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 7,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exRow.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 20,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Warmup,
				SortOrder = 8,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exRow.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 9,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exRow.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 10,
				WorkoutTemplateId = tmpA.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exRow.Id,
				IntensityTarget = 0,
				ListGroupId = groupA.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 11,
				WorkoutTemplateId = tmpA.Id
			}
		);
		await db.SaveChangesAsync();

		var tmpBMuscles = new List<MuscleTypes>();
		tmpBMuscles.AddRange(EnumUtils.FromJoinedIntString<MuscleTypes>(exDeadLift.Muscles));
		tmpBMuscles.AddRange(EnumUtils.FromJoinedIntString<MuscleTypes>(exLatPullDown.Muscles));
		tmpBMuscles.AddRange(EnumUtils.FromJoinedIntString<MuscleTypes>(exShoulderPress.Muscles));

		var tmpB = new WorkoutTemplateEntity
		{
			ExerciseCount = 3,
			MemberId = memberId,
			MusclesTargeted = tmpBMuscles.Titles(),
			SetCount = 12,
			Title = "Sample Workout B"
		};
		db.WorkoutTemplates.Add(tmpB);
		await db.SaveChangesAsync();

		var groupB = new WorkoutTemplateSetGroupEntity()
		{
			WorkoutTemplateId = tmpB.Id,
			MemberId = memberId,
			SortOrder = 0,
			Title = string.Empty
		};
		db.WorkoutTemplateSetGroups.Add(groupB);
		await db.SaveChangesAsync();

		db.WorkoutTemplateSets.AddRange(
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exDeadLift.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 20,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Warmup,
				SortOrder = 0,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exDeadLift.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 1,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exDeadLift.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 2,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exDeadLift.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 3,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exLatPullDown.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 20,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Warmup,
				SortOrder = 4,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exLatPullDown.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 5,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exLatPullDown.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 6,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exLatPullDown.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 7,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exShoulderPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 20,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Warmup,
				SortOrder = 8,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exShoulderPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 9,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exShoulderPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 10,
				WorkoutTemplateId = tmpB.Id
			},
			new WorkoutTemplateSetEntity
			{
				ExerciseId = exShoulderPress.Id,
				IntensityTarget = 0,
				ListGroupId = groupB.Id,
				MemberId = memberId,
				RepMode = RepModes.Exact,
				RepsTarget = 10,
				SetMetricType = SetMetricTypes.WeightReps,
				SetType = WorkoutSetTypes.Standard,
				SortOrder = 11,
				WorkoutTemplateId = tmpB.Id
			}
		);
		await db.SaveChangesAsync();

		// **************
		// Add program
		// **************
		var program = new ProgramEntity
		{
			LengthWeeks = 2,
			MemberId = memberId,
			ProgramType = ProgramTypes.FixedRepeating,
			Title = "Sample Program"
		};
		db.Programs.Add(program);
		await db.SaveChangesAsync();

		db.ProgramTemplates.AddRange(
			new ProgramTemplateEntity
			{
				DayNumber = 1,
				MemberId = memberId,
				ProgramId = program.Id,
				SortOrder = 0,
				WeekNumber = 0,
				WorkoutTemplateId = tmpA.Id
			},
			new ProgramTemplateEntity
			{
				DayNumber = 3,
				MemberId = memberId,
				ProgramId = program.Id,
				SortOrder = 1,
				WeekNumber = 0,
				WorkoutTemplateId = tmpB.Id
			},
			new ProgramTemplateEntity
			{
				DayNumber = 5,
				MemberId = memberId,
				ProgramId = program.Id,
				SortOrder = 2,
				WeekNumber = 0,
				WorkoutTemplateId = tmpA.Id
			},
			new ProgramTemplateEntity
			{
				DayNumber = 1,
				MemberId = memberId,
				ProgramId = program.Id,
				SortOrder = 3,
				WeekNumber = 1,
				WorkoutTemplateId = tmpB.Id
			},
			new ProgramTemplateEntity
			{
				DayNumber = 3,
				MemberId = memberId,
				ProgramId = program.Id,
				SortOrder = 4,
				WeekNumber = 1,
				WorkoutTemplateId = tmpA.Id
			},
			new ProgramTemplateEntity
			{
				DayNumber = 5,
				MemberId = memberId,
				ProgramId = program.Id,
				SortOrder = 5,
				WeekNumber = 1,
				WorkoutTemplateId = tmpB.Id
			}
		);
		await db.SaveChangesAsync();

		// **************
		// Add activity type
		// **************
		var activityWalking = new ActivityTypeEntity()
		{
			Color = "#2155ceff",
			DefaultCadenceUnits = CadenceUnits.SPM,
			DefaultDistanceUnits = DistanceUnits.Mile,
			DefaultEventType = EventTypes.Recreational,
			DefaultElevationUnits = DistanceUnits.Feet,
			DefaultSpeedUnits = SpeedUnits.MilesPerHour,
			MemberId = memberId,
			Title = "Walking"
		};
		db.ActivityTypes.Add(activityWalking);
		await db.SaveChangesAsync();

		// **************
		// Add training session
		// **************
		var sessionWalk = new TrainingSessionEntity()
		{
			Title = "Treadmill Walk: 30 min",
			ActivityTypeId = activityWalking.Id,
			EventType = EventTypes.Recreational,
			MemberId = memberId,
			TargetTimeSeconds = 60 * 30
		};
		db.TrainingSessions.Add(sessionWalk);
		await db.SaveChangesAsync();

		// **************
		// Add training plan
		// **************
		var planWalk = new TrainingPlanEntity()
		{
			Title = "Sample Training Plan",
			MemberId = memberId,
			Description = "A sample plan with a daily 30 min walk on weekdays.",
			LengthWeeks = 1,
			ProgramType = ProgramTypes.FixedRepeating
		};
		db.TrainingPlans.Add(planWalk);
		await db.SaveChangesAsync();

		// **************
		// Add training plan sessions
		// **************
		for (int i = 1; i < 6; i++)
		{
			db.TrainingPlanSessions.Add(new TrainingPlanSessionEntity()
			{
				MemberId = memberId,
				DayNumber = i,
				SortOrder = 0,
				TrainingPlanId = planWalk.Id,
				TrainingSessionId = sessionWalk.Id,
				WeekNumber = 1
			});
		}
		await db.SaveChangesAsync();

		// **************
		// Add metric definitions
		// **************
		var metricGroup = new MetricDefinitionGroupEntity()
		{
			MemberId = memberId,
			Title = "Metrics"
		};
		db.MetricDefinitionGroups.Add(metricGroup);
		await db.SaveChangesAsync();

		db.MetricDefinitions.AddRange(
			new MetricDefinitionEntity
			{
				HighlightColor = "#1841a3ff",
				IsOnDashboard = true,
				ListGroupId = metricGroup.Id,
				MemberId = memberId,
				MetricType = MetricTypes.ExactValue,
				SortOrder = 0,
				SubTitle = "lb",
				Title = "Body Weight",
			},
			new MetricDefinitionEntity
			{
				HighlightColor = "#1841a3ff",
				IsOnDashboard = true,
				ListGroupId = metricGroup.Id,
				MemberId = memberId,
				MetricType = MetricTypes.Counter,
				SortOrder = 1,
				SubTitle = "8 fl.oz.",
				Title = "Hydration"
			}
		);
		await db.SaveChangesAsync();
	}
}
