using Microsoft.AspNetCore.Components;
using MudBlazor;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Studio.Core.Client.Components;

public partial class DialogPhotoViewer
{
	[CascadingParameter] MudDialogInstance MudDialog { get; set; } = null!;

	[Parameter] public string BaseUrl { get; set; } = string.Empty;
	[Parameter] public List<ImageModel> Images { get; set; } = [];

	protected bool DisableBack => currentImageIndex <= 0 || Images.Count == 0;
	protected bool DisableForward => currentImageIndex >= Images.Count - 1;

	private int currentImageIndex = 0;

	public void HandleSwipeEnd(SwipeEventArgs e)
	{
		// Swipe back
		if (e.SwipeDirection == SwipeDirection.LeftToRight)
		{
			if (!DisableBack)
			{
				currentImageIndex--;
				StateHasChanged();
			}
		}
		// Swipe forward
		else if (e.SwipeDirection == SwipeDirection.RightToLeft)
		{
			if (!DisableForward)
			{
				currentImageIndex++;
				StateHasChanged();
			}
		}
	}
}