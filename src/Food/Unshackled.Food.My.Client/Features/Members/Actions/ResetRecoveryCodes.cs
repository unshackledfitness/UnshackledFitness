using MediatR;
using Unshackled.Food.My.Client.Features.Members.Models;
using Unshackled.Studio.Core.Client;
using Unshackled.Studio.Core.Client.Models;

namespace Unshackled.Food.My.Client.Features.Members.Actions;

public class ResetRecoveryCodes
{
	public class Command : IRequest<CommandResult<RecoveryCodesModel>> { }

	public class Handler : BaseMemberHandler, IRequestHandler<Command, CommandResult<RecoveryCodesModel>>
	{
		public Handler(HttpClient httpClient) : base(httpClient) { }

		public async Task<CommandResult<RecoveryCodesModel>> Handle(Command request, CancellationToken cancellationToken)
		{
			return await PostToCommandResultAsync<string, RecoveryCodesModel> ($"{baseUrl}generate-recovery-codes", string.Empty) 
				?? new CommandResult<RecoveryCodesModel>(false, Globals.UnexpectedError);
		}
	}
}
