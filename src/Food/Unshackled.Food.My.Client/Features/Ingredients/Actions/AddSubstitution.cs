using MediatR;
using Unshackled.Food.My.Client.Features.Ingredients.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Ingredients.Actions;

public class AddSubstitution
{
	public class Command : IRequest<CommandResult<ProductSubstitutionModel>>
	{
		public FormSubstitutionModel Model { get; private set; }

		public Command(FormSubstitutionModel model)
		{
			Model = model;
		}
	}

	public class Handler : BaseIngredientHandler, IRequestHandler<Command, CommandResult<ProductSubstitutionModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<ProductSubstitutionModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<FormSubstitutionModel, ProductSubstitutionModel>($"{baseUrl}add-substitution", request.Model)
				?? new CommandResult<ProductSubstitutionModel>(false, Globals.UnexpectedError);
		}
	}
}