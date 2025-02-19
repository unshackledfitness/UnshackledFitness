using AutoMapper;
using MediatR;
using Unshackled.Fitness.Core.Configuration;
using Unshackled.Fitness.Core.Data;
using Unshackled.Fitness.My.Client.Models;

namespace Unshackled.Fitness.My.Features.Members.Actions;

public class GetClientConfiguration
{
	public class Query : IRequest<AppStateConfig> { }

	public class Handler : BaseHandler, IRequestHandler<Query, AppStateConfig>
	{
		private readonly SiteConfiguration siteConfig;
		private readonly StorageSettings storageSettings;

		public Handler(BaseDbContext db, IMapper mapper, SiteConfiguration siteConfig, StorageSettings storageSettings) : base(db, mapper) 
		{
			this.siteConfig = siteConfig;
			this.storageSettings = storageSettings;
		}

		public async Task<AppStateConfig> Handle(Query request, CancellationToken cancellationToken)
		{
			var config = new AppStateConfig(siteConfig.SiteName ?? string.Empty, storageSettings.BaseUrl, storageSettings.MaxUploadInMb);
			await Task.Delay(1, cancellationToken);
			return config;
		}
	}
}
