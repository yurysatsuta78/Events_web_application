using Application.UseCases.Participants.Get.DTOs;
using AutoMapper;
using Domain.Models;

namespace Infrastructure.AutoMapper
{
    public class GetParticipantProfile : Profile
    {
        public GetParticipantProfile() 
        {
            CreateMap<Participant, GetParticipantResponce>()
                .ForMember(res => res.BirthDay, opt => opt.MapFrom(par => par.BirthDay.Date));
        }
    }
}
