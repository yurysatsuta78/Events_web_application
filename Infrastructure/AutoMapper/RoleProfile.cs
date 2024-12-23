using AutoMapper;
using Domain.Models;
using Infrastructure.Entities;

namespace Infrastructure.AutoMapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile() 
        {
            CreateMap<RoleDb, Role>();

            CreateMap<Role, RoleDb>();
        }
    }
}
