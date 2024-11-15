using AutoMapper;
using Unshackled.Fitness.Core.Data;
using Unshackled.Studio.Core.Data;

namespace Unshackled.Fitness.My.Features;

public abstract class BaseHandler
{
	protected readonly FitnessDbContext db;
	protected readonly IMapper mapper;

	public BaseHandler(FitnessDbContext db, IMapper mapper)
	{
		this.db = db;
		this.mapper = mapper;
	}
}
