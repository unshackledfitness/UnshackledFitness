using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.ActivityTypes.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.ActivityTypes;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ActivityTypeEntity, ActivityTypeListModel>()
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
