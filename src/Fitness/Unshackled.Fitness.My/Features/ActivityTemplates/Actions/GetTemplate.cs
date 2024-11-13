using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.ActivityTemplates.Models;

namespace Unshackled.Fitness.My.Features.ActivityTemplates.Actions;

public class GetTemplate
{
	public class Query : IRequest<ActivityTemplateModel>
	{
		public long Id { get; private set; }
		public long MemberId { get; private set; }

		public Query(long memberId, long id)
		{
			Id = id;
			MemberId = memberId;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ActivityTemplateModel>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<ActivityTemplateModel> Handle(Query request, CancellationToken cancellationToken)
		{
			return await mapper.ProjectTo<ActivityTemplateModel>(db.ActivityTemplates
				.AsNoTracking()
				.Where(x => x.Id == request.Id && x.MemberId == request.MemberId))
				.SingleOrDefaultAsync(cancellationToken) ?? new();
		}
	}
}