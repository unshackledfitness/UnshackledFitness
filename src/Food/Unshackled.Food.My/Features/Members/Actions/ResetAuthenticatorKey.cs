using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Unshackled.Food.Core.Data;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Food.My.Features.Members.Actions;

public class ResetAuthenticatorKey
{
	public class Command : IRequest<CommandResult>
	{
		public ClaimsPrincipal User { get; private set; }

		public Command(ClaimsPrincipal user)
		{
			User = user;
		}
	}

	public class Handler : BaseHandler, IRequestHandler<Command, CommandResult>
	{
		private readonly UserManager<UserEntity> userManager; 
		private readonly SignInManager<UserEntity> signInManager;

		public Handler(FoodDbContext db, IMapper mapper, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager) : base(db, mapper)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			var user = await userManager.GetUserAsync(request.User);

			if (user == null)
				return new CommandResult(false, "Invalid user.");

			var disable2faResult = await userManager.SetTwoFactorEnabledAsync(user, false);
			if (!disable2faResult.Succeeded)
				return new CommandResult(false, "Unexpected error occurred disabling 2FA.");

			var identityResult = await userManager.ResetAuthenticatorKeyAsync(user);
			if (!identityResult.Succeeded)
				return new CommandResult(false, "Unexpected error occurred reseting authenticator key.");

			await signInManager.RefreshSignInAsync(user);

			return new CommandResult(true, "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.");
		}
	}
}
