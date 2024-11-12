using System.Text.Json.Serialization;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.Core.Models;

namespace Unshackled.Fitness.My.Client.Features.Dashboard.Models;

public class DashboardStatsModel
{
	public DateTime ToDateUtc { get; set; } = DateTime.UtcNow;
	public List<StatBlockModel> StatBlocks { get; set; } = [];
	public List<int> Years { get; set; } = [];
	public int TotalActivities { get; set; }
	public int TotalWorkouts { get; set; }
	public decimal TotalVolumeLb { get; set; }
	public decimal TotalVolumeKg { get; set; }
	public int TotalSeconds { get; set; }
	public double TotalDistanceMeters { get; set; }

	[JsonIgnore]
	public WeekModel[] Weeks { get; set; } = new WeekModel[53];

	public void Fill(AppSettings settings)
	{
		int statBlockIdx = 0;
		DateTime toDate = ToDateUtc.ToLocalTime().Date;
		DateTime currentDate = ToDateUtc.ToLocalTime().Date.AddYears(-1);
		int firstDayOfWeek = (int)currentDate.DayOfWeek;
		for (int w = 0; w < Weeks.Length; w++)
		{
			Weeks[w] = new()
			{
				Month = currentDate.ToString("MMM")
			};

			for (int d = 0; d < 7; d++)
			{
				Weeks[w].Days[d] = new();
				if (d >= firstDayOfWeek && currentDate < toDate)
				{
					Weeks[w].Days[d].Date = currentDate;
					while (statBlockIdx < StatBlocks.Count && StatBlocks[statBlockIdx].DateCompletedUtc.ToLocalTime().Date == currentDate)
					{
						Weeks[w].Days[d].BlockCount++;

						string color = StatBlocks[statBlockIdx].Type switch
						{
							StatBlockTypes.Activity => settings.ActivityDisplayColor,
							StatBlockTypes.Workout => settings.WorkoutDisplayColor,
							_ => string.Empty
						};

						if (Weeks[w].Days[d].BlockCount == 1)
						{
							Weeks[w].Days[d].BlockTitle = StatBlocks[statBlockIdx].Title;
							Weeks[w].Days[d].BlockColor = color;
						}
						else
						{
							Weeks[w].Days[d].BlockTitle = string.Empty;
							if (Weeks[w].Days[d].BlockColor != color)
							{
								Weeks[w].Days[d].BlockColor = settings.MixedDisplayColor;
							}
						}

						statBlockIdx++;
					}

					currentDate = currentDate.AddDays(1);
				}
			}
			firstDayOfWeek = 0;
		}
	}

	public int GetActiveDays()
	{
		return StatBlocks
			.Select(x => x.DateCompletedUtc.ToString("yyyy-MM-dd"))
			.Distinct()
			.Count();
	}

	public int GetWeekColumnsInMonth(int startIdx)
	{
		string month = Weeks[startIdx].Month;
		int count = 1;
		for (int i = startIdx + 1; i < Weeks.Length; i++)
		{
			if (Weeks[i].Month == month)
				count++;
			else
				break;
		}
		return count;
	}

	public class WeekModel
	{
		public string Month { get; set; } = string.Empty;
		public DayModel[] Days { get; set; } = new DayModel[7];
	}

	public class DayModel
	{
		public DateTime? Date { get; set; }
		public int BlockCount { get; set; }
		public string BlockTitle { get; set; } = string.Empty;
		public string BlockColor { get; set; } = string.Empty;

		public string Description
		{
			get
			{
				if (Date == null)
					return string.Empty;

				if (BlockCount == 0)
					return $"No activities/workouts on {Date.Value.ToString("D")}";

				return BlockCount > 1
					? $"{BlockCount} activities/workouts on {Date.Value.ToString("D")}"
					: $"{BlockTitle} on {Date.Value.ToString("D")}";
			}
		}
	}
}
