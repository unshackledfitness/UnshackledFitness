using Microsoft.AspNetCore.Components;
using Unshackled.Fitness.My.Client.Components;
using Unshackled.Fitness.My.Client.Features.Products.Models;

namespace Unshackled.Fitness.My.Client.Features.Products;

public class FormPropertiesBase : BaseFormComponent<FormProductModel, FormProductModel.Validator>
{
	[Parameter] public List<CategoryModel> Categories { get; set; } = [];
}