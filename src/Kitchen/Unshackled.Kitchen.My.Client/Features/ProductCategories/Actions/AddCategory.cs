using MediatR;
using Unshackled.Kitchen.My.Client.Features.ProductCategories.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Kitchen.My.Client.Features.ProductCategories.Actions;

public class AddCategory
{
	public class Command : IRequest<CommandResult>
	{
		public FormCategoryModel Model { get; private set; }

		public Command(FormCategoryModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseCategoryHandler, IRequestHandler<Command, CommandResult>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync($"{baseUrl}add", request.Model)
				?? new CommandResult(false, Globals.UnexpectedError);
		}
	}
}