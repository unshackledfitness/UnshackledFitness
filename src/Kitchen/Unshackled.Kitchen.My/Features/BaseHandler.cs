using AutoMapper;
using Unshackled.Kitchen.Core.Data;

namespace Unshackled.Kitchen.My.Features;

public abstract class BaseHandler
{
	protected readonly KitchenDbContext db;
	protected readonly IMapper mapper;

	public BaseHandler(KitchenDbContext db, IMapper mapper)
	{
		this.db = db;
		this.mapper = mapper;
	}
}
