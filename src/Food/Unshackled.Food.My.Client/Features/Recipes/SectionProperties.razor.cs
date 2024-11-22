using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Client.Extensions;
using Unshackled.Food.My.Client.Features.Recipes.Actions;
using Unshackled.Food.My.Client.Features.Recipes.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Recipes;

public class SectionPropertiesBase : BaseSectionComponent<Member>
{
	[Inject] protected IDialogService DialogService { get; set; } = default!;
	[Parameter] public RecipeModel Recipe { get; set; } = new();
	[Parameter] public EventCallback<RecipeModel> RecipeChanged { get; set; }
	[Parameter] public EventCallback MakeRecipeClicked { get; set; }
	[Parameter] public decimal Scale { get; set; }

	protected const string FormId = "formRecipeProperties";
	protected bool IsEditing { get; set; } = false;
	protected bool IsSaving { get; set; }
	protected FormRecipeModel Model { get; set; } = new();
	public List<string> RecipeTags { get; set; } = new();

	protected bool DisableControls => IsSaving;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		RecipeTags = Recipe.GetSelectedTags();
	}

	protected async Task HandleEditClicked()
	{
		Model = new()
		{
			Title = Recipe.Title,
			Description = Recipe.Description,
			CookTimeMinutes = Recipe.CookTimeMinutes,
			PrepTimeMinutes = Recipe.PrepTimeMinutes,
			TotalServings = Recipe.TotalServings,
			Sid = Recipe.Sid,
			IsAustralianCuisine = Recipe.IsAustralianCuisine,
			IsCajunCreoleCuisine = Recipe.IsCajunCreoleCuisine,
			IsCaribbeanCuisine = Recipe.IsCaribbeanCuisine,
			IsCentralAfricanCuisine = Recipe.IsCentralAfricanCuisine,
			IsCentralAmericanCuisine = Recipe.IsCentralAmericanCuisine,
			IsCentralAsianCuisine = Recipe.IsCentralAsianCuisine,
			IsCentralEuropeanCuisine = Recipe.IsCentralEuropeanCuisine,
			IsChineseCuisine = Recipe.IsChineseCuisine,
			IsEastAfricanCuisine = Recipe.IsEastAfricanCuisine,
			IsEastAsianCuisine = Recipe.IsEastAsianCuisine,
			IsEasternEuropeanCuisine = Recipe.IsEasternEuropeanCuisine,
			IsFilipinoCuisine = Recipe.IsFilipinoCuisine,
			IsFrenchCuisine = Recipe.IsFrenchCuisine,
			IsFusionCuisine = Recipe.IsFusionCuisine,
			IsGermanCuisine = Recipe.IsGermanCuisine,
			IsGlutenFree = Recipe.IsGlutenFree,
			IsGreekCuisine = Recipe.IsGreekCuisine,
			IsIndianCuisine = Recipe.IsIndianCuisine,
			IsIndonesianCuisine = Recipe.IsIndonesianCuisine,
			IsItalianCuisine = Recipe.IsItalianCuisine,
			IsJapaneseCuisine = Recipe.IsJapaneseCuisine,
			IsKoreanCuisine = Recipe.IsKoreanCuisine,
			IsMexicanCuisine = Recipe.IsMexicanCuisine,
			IsNativeAmericanCuisine = Recipe.IsNativeAmericanCuisine,
			IsNorthAfricanCuisine = Recipe.IsNorthAfricanCuisine,
			IsNorthAmericanCuisine = Recipe.IsNorthAmericanCuisine,
			IsNorthernEuropeanCuisine = Recipe.IsNorthernEuropeanCuisine,
			IsNutFree = Recipe.IsNutFree,
			IsOceanicCuisine = Recipe.IsOceanicCuisine,
			IsPakistaniCuisine = Recipe.IsPakistaniCuisine,
			IsPolishCuisine = Recipe.IsPolishCuisine,
			IsPolynesianCuisine = Recipe.IsPolynesianCuisine,
			IsRussianCuisine = Recipe.IsRussianCuisine,
			IsSoulFoodCuisine = Recipe.IsSoulFoodCuisine,
			IsSouthAfricanCuisine = Recipe.IsSouthAfricanCuisine,
			IsSouthAmericanCuisine = Recipe.IsSouthAmericanCuisine,
			IsSouthAsianCuisine = Recipe.IsSouthAsianCuisine,
			IsSoutheastAsianCuisine = Recipe.IsSoutheastAsianCuisine,
			IsSouthernEuropeanCuisine = Recipe.IsSouthernEuropeanCuisine,
			IsSpanishCuisine = Recipe.IsSpanishCuisine,
			IsTexMexCuisine = Recipe.IsTexMexCuisine,
			IsThaiCuisine = Recipe.IsThaiCuisine,
			IsVegan = Recipe.IsVegan,
			IsVegetarian = Recipe.IsVegetarian,
			IsVietnameseCuisine = Recipe.IsVietnameseCuisine,
			IsWestAfricanCuisine = Recipe.IsWestAfricanCuisine,
			IsWestAsianCuisine = Recipe.IsWestAsianCuisine,
			IsWesternEuropeanCuisine = Recipe.IsWesternEuropeanCuisine
		};

		IsEditing = await UpdateIsEditingSection(true);
	}

	protected async Task HandleDeleteClicked()
	{
		bool? confirm = await DialogService.ShowMessageBox(
				"Confirm Delete",
				"Are you sure you want to delete this recipe?",
				yesText: "Yes", cancelText: "No");

		if (confirm.HasValue && confirm.Value)
		{
			await UpdateIsEditingSection(true);

			var result = await Mediator.Send(new DeleteRecipe.Command(Recipe.Sid));
			ShowNotification(result);
			if (result.Success)
			{
				NavManager.NavigateTo("/recipes");
			}
			await UpdateIsEditingSection(false);
		}
	}

	protected async Task HandleEditCancelClicked()
	{
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleFormSubmitted(FormRecipeModel model)
	{
		IsSaving = true;
		var result = await Mediator.Send(new UpdateRecipeProperties.Command(model));
		ShowNotification(result);
		if (result.Success)
		{
			await RecipeChanged.InvokeAsync(result.Payload);
		}
		IsSaving = false;
		IsEditing = await UpdateIsEditingSection(false);
	}

	protected async Task HandleMakeRecipeClicked()
	{
		await MakeRecipeClicked.InvokeAsync();
	}
}