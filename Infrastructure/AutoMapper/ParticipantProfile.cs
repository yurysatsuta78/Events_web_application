using Application.DTO.Response.Participant;
using AutoMapper;
using Domain.Models;
using Infrastructure.Entities;

namespace Infrastructure.AutoMapper
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile() 
        {
            CreateMap<Participant, ParticipantDb>()
                .ForMember(db => db.Roles, opt => opt.Ignore());

            CreateMap<ParticipantDb, Participant>();

            CreateMap<ParticipantDb, GetParticipantDto>()
                .ForMember(dto => dto.Events, opt => opt.MapFrom(db => db.Events.Select(ev => ev.Name).ToList()));
        }
    }
}
