using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Unshackled.Food.Core.Data;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client.Models;
using Unshackled.Studio.Core.Data.Entities;

namespace Unshackled.Food.My.Features.Members.Actions;

public class AddPassword
{
	public class Command : IRequest<CommandResult>
	{
		public ClaimsPrincipal User { get; private set; }
		public FormSetPasswordModel Model { get; private set; }

		public Command(ClaimsPrincipal user, FormSetPasswordModel model)
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
			if (hasPassword)
				return new CommandResult(false, "You already have a password.");

			var addPasswordResult = await userManager.AddPasswordAsync(user, request.Model.NewPassword);
			if (!addPasswordResult.Succeeded)
			{
				return new CommandResult(false, $"Error: {string.Join(",", addPasswordResult.Errors.Select(error => error.Description))}");
			}

			await signInManager.RefreshSignInAsync(user);

			return new CommandResult(true, "Your password has been set.");
		}
	}
}
