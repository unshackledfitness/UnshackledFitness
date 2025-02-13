using MediatR;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Recipes.Actions;

public class DeleteImage
{
	public class Command : IRequest<CommandResult<string>>
	{
		public string Sid { get; private set; }

		public Command(string sid)
		{
			Sid = sid;
		}
	}

	public class Handler : BaseRecipeHandler, IRequestHandler<Command, CommandResult<string>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<string>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<string, string>($"{baseUrl}delete-image", request.Sid)
				?? new CommandResult<string>(false, Globals.UnexpectedError);
		}
	}
}
