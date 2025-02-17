using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.Activities.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Activities;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ActivityEntity, ActivityListModel>()
			.ForMember(d => d.ActivityTypeColor, m => m.MapFrom(s => s.ActivityType != null ? s.ActivityType.Color : string.Empty))
			.ForMember(d => d.ActivityTypeSid, m => m.MapFrom(s => s.ActivityTypeId.Encode()))
			.ForMember(d => d.ActivityTypeTitle, m => m.MapFrom(s => s.ActivityType != null ? s.ActivityType.Title : string.Empty))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ActivityEntity, ActivityModel>()
			.ForMember(d => d.ActivityTypeColor, m => m.MapFrom(s => s.ActivityType != null ? s.ActivityType.Color : string.Empty))
			.ForMember(d => d.ActivityTypeSid, m => m.MapFrom(s => s.ActivityTypeId.Encode()))
			.ForMember(d => d.ActivityTypeTitle, m => m.MapFrom(s => s.ActivityType != null ? s.ActivityType.Title : string.Empty))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()))
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<ActivityTypeEntity, ActivityTypeListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
