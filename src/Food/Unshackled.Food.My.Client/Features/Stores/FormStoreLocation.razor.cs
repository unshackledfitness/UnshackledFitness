using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Stores.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Stores;

public class FormStoreLocationBase : BaseFormComponent<FormStoreLocationModel, FormStoreLocationModel.Validator>
{
	[Parameter] public string SubmitButtonLabel { get; set; } = "Save";
}