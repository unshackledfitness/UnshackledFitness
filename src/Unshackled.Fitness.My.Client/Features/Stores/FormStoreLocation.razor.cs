using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Stores.Models;

namespace Unshackled.Fitness.My.Client.Features.Stores;

public class FormStoreLocationBase : BaseFormComponent<FormStoreLocationModel, FormStoreLocationModel.Validator>
{
	[Parameter] public string SubmitButtonLabel { get; set; } = "Save";
}