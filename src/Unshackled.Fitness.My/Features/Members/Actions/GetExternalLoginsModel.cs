using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Fitness.My.Features.Members.Actions;

public class GetExternalLoginsModel
{
	public class Query : IRequest<ExternalLoginsModel>
	{
		public ClaimsPrincipal User { get; private set; }

		public Query(ClaimsPrincipal user)
		{
			User = user;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Query, ExternalLoginsModel>
	{
		private readonly UserManager<UserEntity> userManager;
		private readonly SignInManager<UserEntity> signInManager;

		public Handler(FitnessDbContext db, IMapper mapper, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) : base(db, mapper)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public async Task<ExternalLoginsModel> Handle(Query request, CancellationToken cancellationToken)
		{
			ExternalLoginsModel model = new();
			var user = await userManager.GetUserAsync(request.User);

			if (user != null)
			{
				model.HasPasswordSet = await userManager.HasPasswordAsync(user);

				model.CurrentLogins = (await userManager.GetLoginsAsync(user))
					.Select(x => new FormProviderModel
					{
						LoginProvider = x.LoginProvider,
						ProviderDisplayName = x.ProviderDisplayName,
						ProviderKey = x.ProviderKey
					})
					.ToList();

				model.OtherLogins = (await signInManager.GetExternalAuthenticationSchemesAsync())
					.Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
					.Select(x => new FormProviderModel
					{
						LoginProvider = x.Name,
						ProviderDisplayName = x.DisplayName,
						ProviderKey = x.Name
					})
					.ToList();
			}

			return model;
		}
	}
}
