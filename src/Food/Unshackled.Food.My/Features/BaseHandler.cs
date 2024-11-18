using AutoMapper;
using Unshackled.Food.Core.Data;

namespace Unshackled.Food.My.Features;

public abstract class BaseHandler
{
	protected readonly FoodDbContext db;
	protected readonly IMapper mapper;

	public BaseHandler(FoodDbContext db, IMapper mapper)
	{
		this.db = db;
		this.mapper = mapper;
	}
}
