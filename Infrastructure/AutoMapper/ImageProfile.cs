using AutoMapper;
using Domain.Models;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AutoMapper
{
    public class ImageProfile : Profile
    {
        public ImageProfile() 
        {
            CreateMap<Image, ImageDb>();
            //.ForMember(db => db.Id, opt => opt.MapFrom(im => im.Id))
            //.ForMember(db => db.ImagePath, opt => opt.MapFrom(im => im.ImagePath))
            //.ForMember(db => db.Event, opt => opt.Ignore());

            CreateMap<ImageDb, Image>();
                //.ForMember(im => im.Id, opt => opt.MapFrom(db => db.Id))
                //.ForMember(im => im.ImagePath, opt => opt.MapFrom(db => db.ImagePath));
        }
    }
}
