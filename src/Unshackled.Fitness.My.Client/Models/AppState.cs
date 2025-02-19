namespace Unshackled.Fitness.My.Client.Models;

public class AppState
{
	public Member ActiveMember { get; private set; } = new();
	public AppStateConfig Config { get; private set; } = new();
	public bool IsMemberLoaded { get; private set; } = false;
	public bool IsServerError { get; private set; } = false;
	public List<MakeItRecipeModel> MakeItRecipes { get; private set; } = [];
	public int MakeItIndex { get; private set; }
	public virtual string StoragePrefix => "uf_";

	public event Action? OnActiveMemberChange;
	public event Action? OnConfigurationChange;
	public event Action? OnMakeItRecipesChanged;
	public event Action? OnMemberLoadedChange;
	public event Action? OnSaveMakeItRecipeChanges;
	public event Action? OnServerErrorChange;
	public event Action? OnThemeChange;

	public virtual void AddMakeItRecipe(MakeItRecipeModel recipe)
	{
		var existing = MakeItRecipes.Where(x => x.Sid == recipe.Sid).FirstOrDefault();
		if (existing == null)
		{
			recipe.SortOrder = MakeItRecipes.Count;
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

		if (MakeItIndex > 0)
			MakeItIndex--;

		for (int i = 0; i < MakeItRecipes.Count; i++)
		{
			MakeItRecipes[i].SortOrder = i;
		}
		OnMakeItRecipesChanged?.Invoke();
	}

	public void SaveMakeItRecipeChanges()
	{
		OnSaveMakeItRecipeChanges?.Invoke();
	}

	public virtual void SetActiveMember(Member member)
	{
		bool themeChanged = false;
		if (member.AppTheme != ActiveMember.AppTheme)
		{
			themeChanged = true;
		}

		ActiveMember = member;
		SetMemberLoaded(true);
		OnActiveMemberChange?.Invoke();
		if (themeChanged)
			OnThemeChange?.Invoke();
	}

	public virtual void SetConfiguration(AppStateConfig config)
	{
		Config = config;
		OnConfigurationChange?.Invoke();
	}

	public virtual void SetMakeItRecipes(List<MakeItRecipeModel> recipes)
	{
		MakeItRecipes = recipes;
	}

	public virtual void SetMemberLoaded(bool loaded)
	{
		if (IsMemberLoaded != loaded)
		{
			IsMemberLoaded = loaded;
			OnMemberLoadedChange?.Invoke();
		}
	}

	public virtual void SetServerError()
	{
		IsServerError = true;
		OnServerErrorChange?.Invoke();
	}

	public void UpdateIndex(int index)
	{
		MakeItIndex = index;
	}
}