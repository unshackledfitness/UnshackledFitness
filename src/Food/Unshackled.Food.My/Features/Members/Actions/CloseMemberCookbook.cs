using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Models;
using Unshackled.Food.My.Extensions;
using Unshackled.Studio.Core.Data.Extensions;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Food.My.Features.Members.Actions;

public class CloseMemberCookbook
{
	public class Command : IRequest<Member?>
	{
		public long MemberId { get; private set; }
		public string CookbookSid { get; private set; }

		public Command(long memberId, string cookbookSid)
		{
			MemberId = memberId;
			CookbookSid = cookbookSid;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, Member?>
	{
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<Member?> Handle(Command request, CancellationToken cancellationToken)
		{
			var memberEntity = await db.Members
				.Where(s => s.Id == request.MemberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (memberEntity == null)
				return null;

			long cookbookId = request.CookbookSid.DecodeLong();

			if (cookbookId == 0)
				return null;

			await db.ClearMetaKey(request.MemberId, FoodGlobals.MetaKeys.ActiveCookbookId);

			var member = await db.GetMember(memberEntity);

			return member;
		}
	}
}
