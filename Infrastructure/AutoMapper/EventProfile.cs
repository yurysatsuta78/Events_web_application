using Application.DTO.Response.Event;
using AutoMapper;
using Domain.Models;
using Infrastructure.Entities;
namespace Infrastructure.AutoMapper
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDb, Event>();

            CreateMap<EventDb, GetEventDto>()
                .ForMember(dto => dto.ImagePaths, opt => opt.MapFrom(ev => ev.Images.Select(im => im.ImagePath).ToList()))
                .ForMember(dto => dto.Participants, opt => opt.MapFrom(ev => ev.Participants.Count));

            CreateMap<Event, EventDb>();

            CreateMap<Event, GetEventDto>()
                .ForMember(dto => dto.ImagePaths, opt => opt.MapFrom(ev => ev.Images.Select(im => im.ImagePath).ToList()))
                .ForMember(dto => dto.Participants, opt => opt.MapFrom(ev => ev.Participants.Count));

            CreateMap<Event, GetEventParticipantsDto>()
                .ForMember(dto => dto.EventName, opt => opt.MapFrom(ev => ev.Name))
                .ForMember(dto => dto.ParticipantIds, opt => opt.MapFrom(ev => ev.Participants
                    .Select(pa => pa.Id).ToList()));
        }
    }
}
