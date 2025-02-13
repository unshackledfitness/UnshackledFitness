using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.Cookbooks.Models;
using Unshackled.Fitness.My.Extensions;

namespace Unshackled.Fitness.My.Features.Cookbooks;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<CookbookEntity, CookbookListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()));
		CreateMap<CookbookEntity, CookbookModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()));
		CreateMap<CookbookInviteEntity, InviteListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<CookbookMemberEntity, MemberListModel>()
			.ForMember(d => d.MemberSid, m => m.MapFrom(s => s.MemberId.Encode()))
			.ForMember(d => d.Email, m => m.MapFrom(s => s.Member != null ? s.Member.Email : string.Empty));
	}
}
