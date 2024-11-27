using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Unshackled.Food.Core.Data;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Food.My.Features.Members.Actions;

public class RemoveLoginProvider
{
	public class Command : IRequest<CommandResult>
	{
		public ClaimsPrincipal User { get; private set; }
		public FormProviderModel Model { get; private set; }

		public Command(ClaimsPrincipal user, FormProviderModel model)
		{
			User = user;
			Model = model;
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

			bool hasPassword = await userManager.HasPasswordAsync(user);
			if (!hasPassword)
				return new CommandResult(false, "You do not have a local password set.");

			var result = await userManager.RemoveLoginAsync(user, request.Model.LoginProvider, request.Model.ProviderKey);
			if (result.Succeeded)
			{
				await signInManager.RefreshSignInAsync(user);
				return new CommandResult(true, $"{request.Model.LoginProvider} login removed.");
			}

			return new CommandResult(false, "Could not remove the external login provider.");
		}
	}
}
