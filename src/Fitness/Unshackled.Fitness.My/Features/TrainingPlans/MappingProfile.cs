using AutoMapper;
using Unshackled.Fitness.Core.Data.Entities;
using Unshackled.Fitness.My.Client.Features.TrainingPlans.Models;
using Unshackled.Studio.Core.Server.Extensions;

namespace Unshackled.Fitness.My.Features.TrainingPlans;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<TrainingPlanEntity, PlanListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<TrainingPlanEntity, PlanModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
		CreateMap<TrainingPlanSessionEntity, PlanSessionModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()))
			.ForMember(d => d.TrainingPlanSid, m => m.MapFrom(s => s.TrainingPlanId.Encode()))
			.ForMember(d => d.TrainingSessionSid, m => m.MapFrom(s => s.TrainingSessionId.Encode()))
			.ForMember(d => d.TrainingSessionName, m => m.MapFrom(s => s.Session != null ? s.Session.Title : string.Empty));
		CreateMap<TrainingSessionEntity, SessionListModel>()
			.ForMember(d => d.Sid, m => m.MapFrom(s => s.Id.Encode()));
	}
}
