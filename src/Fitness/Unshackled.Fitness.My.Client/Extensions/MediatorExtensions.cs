using MediatR;
using Unshackled.Fitness.Core.Models;
using Unshackled.Fitness.My.Client.Features.Members.Actions;
using Unshackled.Studio.Core.Client.Enums;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Extensions;

public static class MediatorExtensions
{
	public static async Task CloseMemberCookbook(this IMediator mediator, string cookbookSid)
	{
		await mediator.Send(new CloseMemberCookbook.Command(cookbookSid));
	}

	public static async Task CloseMemberHousehold(this IMediator mediator, string householdSid)
	{
		await mediator.Send(new CloseMemberHousehold.Command(householdSid));
	}

	public static async Task GetActiveMember(this IMediator mediator)
	{
		await mediator.Send(new GetActiveMember.Query());
	}

	public static async Task OpenMemberCookbook(this IMediator mediator, string cookbookSid)
	{
		await mediator.Send(new OpenMemberCookbook.Command(cookbookSid));
	}

	public static async Task OpenMemberHousehold(this IMediator mediator, string householdSid)
	{
		await mediator.Send(new OpenMemberHousehold.Command(householdSid));
	}

	public static async Task<CommandResult> SaveSettings(this IMediator mediator, AppSettings settings)
	{
		return await mediator.Send(new SaveSettings.Command(settings));
	}

	public static async Task<CommandResult> SetTheme(this IMediator mediator, Themes theme)
	{
		return await mediator.Send(new SetTheme.Command(theme));
	}
}
