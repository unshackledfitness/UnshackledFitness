using Unshackled.Food.Core.Enums;
using Unshackled.Studio.Core.Client.Enums;

namespace Unshackled.Food.Core.Models;

public class AppSettings : ICloneable
{
	// Shopping Lists
	public bool HideIsInCart { get; set; } = false;

	public object Clone()
	{
		return new AppSettings
		{
			HideIsInCart = HideIsInCart
		};
	}
}
