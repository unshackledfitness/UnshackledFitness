using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.Households.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Households;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<HouseholdEntity, HouseholdListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()));
		CreateMap<HouseholdEntity, HouseholdModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()));
		CreateMap<HouseholdInviteEntity, InviteListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<HouseholdMemberEntity, MemberListModel>()
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()))
			.ForMember(d => d.Email, m => m.MapFrom(s => s.Member != null ? s.Member.Email : string.Empty));
	}
}
