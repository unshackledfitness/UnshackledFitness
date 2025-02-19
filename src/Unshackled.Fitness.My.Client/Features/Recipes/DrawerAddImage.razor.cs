using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Extensions;
using Unshackled.Fitness.My.Client.Features.Recipes.Actions;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes;

public class DrawerAddImageBase : BaseComponent
{
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

			long maxFileSize = State.Config.MaxUploadInMb * 1024 * 1024;

			var result = await file.ProcessFormFile([".jpg", ".jpeg"], maxFileSize);
			if (!result.Success)
			{
				UploadError = result.Message ?? "Invalid file.";
				IsUploading = false;
			}

			var resizedfile = await file.RequestImageFileAsync(e.File.ContentType, Globals.MaxImageWidth, int.MaxValue);

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