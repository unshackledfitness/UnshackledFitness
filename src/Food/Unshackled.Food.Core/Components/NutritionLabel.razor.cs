using Microsoft.AspNetCore.Components;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.Core.Extensions;
using Unshackled.Food.Core.Models.Recipes;

namespace Unshackled.Food.Core.Components;

public partial class NutritionLabel
{
	[Parameter] public INutrition Item { get; set; } = default!;
	[Parameter] public int xs { get; set; } = 12;
	[Parameter] public int sm { get; set; }
	[Parameter] public int md { get; set; }
	[Parameter] public int lg { get; set; }
	[Parameter] public int xl { get; set; }
}