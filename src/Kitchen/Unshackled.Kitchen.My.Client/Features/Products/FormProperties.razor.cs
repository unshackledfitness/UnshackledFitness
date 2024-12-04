using Microsoft.AspNetCore.Components;
using Unshackled.Kitchen.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Kitchen.My.Client.Features.Products;

public class FormPropertiesBase : BaseFormComponent<FormProductModel, FormProductModel.Validator>
{
	[Parameter] public List<ProductCategoryModel> Categories { get; set; } = [];
}