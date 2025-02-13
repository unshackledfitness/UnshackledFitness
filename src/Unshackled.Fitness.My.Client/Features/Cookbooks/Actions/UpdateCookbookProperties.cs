using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Cookbooks.Actions;

public class UpdateCookbookProperties
{
	public class Command : IRequest<CommandResult<CookbookModel>>
	{
		public FormCookbookModel Model { get; private set; }

		public Command(FormCookbookModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCookbookHandler, IRequestHandler<Command, CommandResult<CookbookModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<CookbookModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormCookbookModel, CookbookModel>($"{baseUrl}update", request.Model)
				?? new CommandResult<CookbookModel>(false, Globals.UnexpectedError);
		}
	}
}
