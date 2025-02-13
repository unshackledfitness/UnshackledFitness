using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Extensions;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

public class UpdateMember
{
	public class Command : IRequest<CommandResult>
	{
		public long MemberId { get; private set; }
		public FormMemberModel Model { get; private set; }

		public Command(long memberId, FormMemberModel model)
		{
			MemberId = memberId;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			long cookbookId = request.Model.CookbookSid.DecodeLong();

			if (cookbookId == 0)
				return new CommandResult(false, "Invalid cookbook ID.");

			if (!await db.HasCookbookPermission(cookbookId, request.MemberId, PermissionLevels.Admin))
				return new CommandResult(false, FitnessGlobals.PermissionError);

			long memberId = request.Model.MemberSid.DecodeLong();

			if (memberId == 0)
				return new CommandResult(false, "Invalid member ID.");

			if (await db.Cookbooks.Where(x => x.Id == cookbookId && x.MemberId == memberId).AnyAsync(cancellationToken))
				return new CommandResult(false, "Cannot change the cookbook owner's permissions.");

			CookbookMemberEntity? member = await db.CookbookMembers
				.Where(x => x.CookbookId == cookbookId && x.MemberId == memberId)
				.SingleOrDefaultAsync(cancellationToken);

			if (member == null)
				return new CommandResult(false, "Invalid cookbook member.");

			// Update member
			member.PermissionLevel = request.Model.PermissionLevel;
			await db.SaveChangesAsync(cancellationToken);

			return new CommandResult(true, "Cookbook member updated.");
		}
	}
}