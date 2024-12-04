using AutoMapper;
using Unshackled.Kitchen.Core.Models;
using Unshackled.Studio.Core.Data.Entities;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Kitchen.My.Features.Members;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<MemberEntity, Member>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
