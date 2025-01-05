using Application.UseCases.Events.Get.DTOs;
using AutoMapper;
using Domain.Models;
namespace Infrastructure.AutoMapper
{
    public class GetEventProfile : Profile
    {
        public GetEventProfile()
        {
            CreateMap<Event, GetEventResponce>()
                .ForMember(res => res.ImagePaths, opt => opt.MapFrom(ev => ev.Images.Select(im => im.ImagePath).ToList()))
                .ForMember(res => res.Participants, opt => opt.MapFrom(ev => ev.Participants.Count));
        }
    }
}
