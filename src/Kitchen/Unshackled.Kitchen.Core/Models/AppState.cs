using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.Core.Models;

public class AppState : BaseAppState, IAppState
{
	public event Action? OnMakeItRecipesChanged;
	public event Action? OnSaveMakeItRecipeChanges;

	public override required IMember ActiveMember { get; set; } = new Member();	
	public override string StoragePrefix => "uk_";	
	public List<MakeItRecipeModel> MakeItRecipes { get; private set; } = [];
	public int MakeItIndex { get; private set; } = 1;

	public virtual void AddMakeItRecipe(MakeItRecipeModel recipe)
	{
		var existing = MakeItRecipes.Where(x => x.Sid == recipe.Sid).FirstOrDefault();
		if (existing == null)
		{
			recipe.SortOrder = MakeItRecipes.Count + 1;
			MakeItRecipes.Add(recipe);
			MakeItIndex = recipe.SortOrder;
			OnMakeItRecipesChanged?.Invoke();
		}
		else
		{
			MakeItIndex = existing.SortOrder;
		}
	}

	public virtual void RemoveMakeItRecipe(MakeItRecipeModel recipe)
	{
		MakeItRecipes.Remove(recipe);
		
		if (MakeItIndex > 1)
			MakeItIndex--;

		for (int i = 0; i < MakeItRecipes.Count; i++)
		{
			MakeItRecipes[i].SortOrder = i + 1;
		}
		OnMakeItRecipesChanged?.Invoke();
	}

	public virtual void SetMakeItRecipes(List<MakeItRecipeModel> recipes)
	{
		MakeItRecipes = recipes;
	}

	public void SaveMakeItRecipeChanges()
	{
		OnSaveMakeItRecipeChanges?.Invoke();
	}

	public void UpdateIndex(int index)
	{
		MakeItIndex = index;
	}
}