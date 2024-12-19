using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Unshackled.Kitchen.Core;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Kitchen.My.Client.Features.Recipes.Actions;
using Unshackled.Studio.Core.Client.Components;
using Unshackled.Studio.Core.Client.Configuration;
using Unshackled.Studio.Core.Client.Extensions;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.Recipes;

public class DrawerAddImageBase : BaseComponent<Member>
{
	[Inject] public StorageSettings StorageSettings { get; set; } = default!;
	[Parameter] public string RecipeSid { get; set; } = string.Empty;
	[Parameter] public EventCallback<ImageModel> OnUploadCompleted { get; set; }

	protected bool IsUploading { get; set; } = false;
	protected string UploadError { get; set; } = string.Empty;

	protected async Task HandleInputFileChange(InputFileChangeEventArgs e)
	{
		if (e.FileCount > 0)
		{
			IsUploading = true;

			var file = e.File;

			long maxFileSize = StorageSettings.MaxUploadInMb * 1024 * 1024;

			var result = await file.ProcessFormFile([".jpg", ".jpeg"], maxFileSize);
			if (!result.Success)
			{
				UploadError = result.Message ?? "Invalid file.";
				IsUploading = false;
			}

			var resizedfile = await file.RequestImageFileAsync(e.File.ContentType, KitchenGlobals.MaxImageWidth, int.MaxValue);

			using var content = new MultipartFormDataContent();
			var fileContent = new StreamContent(resizedfile.OpenReadStream(maxFileSize));
			fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
			content.Add(
				content: fileContent,
				name: "\"file\"",
				fileName: file.Name
			);

			var uploaded = await Mediator.Send(new UploadImage.Command(RecipeSid, content));
			if (uploaded.Success)
			{
				ShowNotification(uploaded);
				await OnUploadCompleted.InvokeAsync(uploaded.Payload);
			}
			else
			{
				UploadError = uploaded.Message ?? "Could not upload the image.";
			}

			IsUploading = false;
		}
	}
}