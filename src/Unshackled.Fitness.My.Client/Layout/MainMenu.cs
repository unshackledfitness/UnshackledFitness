using MudBlazor;

namespace Unshackled.Fitness.My.Client.Layout;

public class MainMenu
{
	public int groupMenuIdx = 4;
	public List<MenuItem> Menus { get; set; }

	public MainMenu()
	{
		Menus = [
			new MenuItem { Url = "/", Title = "Dashboard", Icon = Icons.Material.Filled.Home, Match = MenuItem.MatchAll },
			new MenuItem { Url = "/calendar", Title = "Calendar", Icon = Icons.Material.Filled.CalendarMonth, Match = MenuItem.MatchPrefix },

			new MenuItem { Type = MenuItem.Types.Divider },

			new MenuItem
			{
				Id = "activities",
				Icon = Icons.Material.Filled.DirectionsRun,
				Title = "Activities",
				Type = MenuItem.Types.Group,
				Items =
				[
					new MenuItem { Url = "/activities", Title = "Activities", Match = MenuItem.MatchPrefix },
					new MenuItem { Url = "/activity-types", Title = "Activity Types", Match = MenuItem.MatchPrefix }
				]
			},
			new MenuItem
			{
				Id = "strength",
				Icon = Icons.Material.Filled.FitnessCenter,
				Title = "Strength Training",
				Type = MenuItem.Types.Group,
				Items =
				[
					new MenuItem { Url = "/exercises", Title = "Exercises", Match = MenuItem.MatchPrefix },
					new MenuItem { Url = "/templates", Title = "Templates", Match = MenuItem.MatchPrefix },
					new MenuItem { Url = "/programs", Title = "Programs", Match = MenuItem.MatchPrefix },
					new MenuItem { Url = "/workouts", Title = "Workouts", Match = MenuItem.MatchPrefix }
				]
			}
		];
	}
}
