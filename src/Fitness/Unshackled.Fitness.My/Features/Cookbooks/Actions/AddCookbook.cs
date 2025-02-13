using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.Core.Enums;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.My.Features.Cookbooks.Actions;

public class AddCookbook
{
	public class Command : IRequest<CommandResult<CookbookListModel>>
	{
		public long MemberId { get; private set; }
		public bool HasActive { get; private set; }
		public FormCookbookModel Model { get; private set; }

		public Command(long memberId, bool hasActive, FormCookbookModel model)
		{
			MemberId = memberId;
			HasActive = hasActive;
			Model = model;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult<CookbookListModel>>
	{
		public Handler(FitnessDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<CookbookListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create new cookbook
				CookbookEntity cookbook = new()
				{
					MemberId = request.MemberId,
					Title = request.Model.Title.Trim()
				};
				db.Cookbooks.Add(cookbook);
				await db.SaveChangesAsync(cancellationToken);

				// Add member to cookbook as Admin
				CookbookMemberEntity cookbookMember = new()
				{
					CookbookId = cookbook.Id,
					MemberId = request.MemberId,
					PermissionLevel = PermissionLevels.Admin
				};
				db.CookbookMembers.Add(cookbookMember);
				await db.SaveChangesAsync(cancellationToken);

				if(!request.HasActive)
				{
					db.MemberMeta.Add(new MemberMetaEntity
					{
						MemberId = request.MemberId,
						MetaKey = FitnessGlobals.MetaKeys.ActiveCookbookId,
						MetaValue = cookbook.Id.ToString()
					});
					await db.SaveChangesAsync(cancellationToken);
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<CookbookListModel>(true, "Cookbook added.", mapper.Map<CookbookListModel>(cookbook));
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<CookbookListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}