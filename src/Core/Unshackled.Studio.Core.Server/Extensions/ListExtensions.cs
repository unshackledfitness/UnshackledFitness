using HashidsNet;
using Unshackled.Studio.Core.Client.Configuration;

namespace Unshackled.Studio.Core.Server.Extensions;

public static class ListExtensions
{
	public static List<long> DecodeLong(this List<string> values)
	{
		var hashids = new Hashids(HashIdSettings.Salt, HashIdSettings.MinLength, HashIdSettings.Alphabet);
		List<long> list = new();
		foreach (var item in values)
		{
			try
			{
				long val = hashids.DecodeSingleLong(item);
				list.Add(val);
			}
			catch (NoResultException) { }
		}
		return list;
	}
}