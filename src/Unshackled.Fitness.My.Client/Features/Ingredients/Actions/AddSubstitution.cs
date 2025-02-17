using MediatR;
using Unshackled.Fitness.Core;
using Unshackled.Fitness.My.Client.Features.Ingredients.Models;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Client.Features.Ingredients.Actions;

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