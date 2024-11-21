using Microsoft.AspNetCore.Components;
using Unshackled.Food.My.Client.Features.Products.Models;
using Unshackled.Studio.Core.Client.Components;

namespace Unshackled.Food.My.Client.Features.Products;

public class FormPropertiesBase : BaseFormComponent<FormProductModel, FormProductModel.Validator>
{
	[Parameter] public List<ProductCategoryModel> Categories { get; set; } = [];
}