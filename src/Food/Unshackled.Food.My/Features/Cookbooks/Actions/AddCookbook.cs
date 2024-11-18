using AutoMapper;
using MediatR;
using Unshackled.Food.Core;
using Unshackled.Food.Core.Data;
using Unshackled.Food.Core.Data.Entities;
using Unshackled.Food.Core.Enums;
using Unshackled.Food.My.Client.Features.Cookbooks.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Food.My.Features.Cookbooks.Actions;

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
		public Handler(FoodDbContext db, IMapper mapper) : base(db, mapper) { }

		public async Task<CommandResult<CookbookListModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

			try
			{
				// Create new group
				CookbookEntity group = new()
				{
					MemberId = request.MemberId,
					Title = request.Model.Title.Trim()
				};
				db.Cookbooks.Add(group);
				await db.SaveChangesAsync();

				// Add member to group as Admin
				CookbookMemberEntity groupMember = new()
				{
					CookbookId = group.Id,
					MemberId = request.MemberId,
					PermissionLevel = PermissionLevels.Admin
				};
				db.CookbookMembers.Add(groupMember);
				await db.SaveChangesAsync(cancellationToken);

				if(!request.HasActive)
				{
					db.MemberMeta.Add(new MemberMetaEntity
					{
						MemberId = request.MemberId,
						MetaKey = FoodGlobals.MetaKeys.ActiveCookbookId,
						MetaValue = group.Id.ToString()
					});
					await db.SaveChangesAsync();
				}

				await transaction.CommitAsync(cancellationToken);

				return new CommandResult<CookbookListModel>(true, "Cookbook added.", mapper.Map<CookbookListModel>(group));
			}
			catch
			{
				await transaction.RollbackAsync(cancellationToken);
				return new CommandResult<CookbookListModel>(false, Globals.UnexpectedError);
			}
		}
	}
}