using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Unshackled.Kitchen.Core.Data;
using Unshackled.Kitchen.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Kitchen.My.Features.Members.Actions;

public class GetCurrentUserEmail
{
	public class Query : IRequest<CurrentUserEmailModel>
	{
		public ClaimsPrincipal User { get; private set; }

		public Query(ClaimsPrincipal user)
		{
			User = user;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, CurrentUserEmailModel>
	{
		private readonly UserManager<UserEntity> userManager;

		public Handler(KitchenDbContext db, IMapper mapper, UserManager<UserEntity> userManager) : base(db, mapper) 
		{
			this.userManager = userManager;
		}

		public async Task<CurrentUserEmailModel> Handle(Query request, CancellationToken cancellationToken)
		{
			var user = await userManager.GetUserAsync(request.User);

			if (user == null)
				return new();

			return new()
			{
				Email = user.Email!,
				IsEmailVerified = await userManager.IsEmailConfirmedAsync(user)
			};
		}
	}
}
