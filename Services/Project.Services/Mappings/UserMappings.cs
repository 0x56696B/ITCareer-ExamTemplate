using System;
using AutoMapper;
using Project.Data.Models;
using Project.Services.Models.User;

namespace Project.Services.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<RegisterServiceModel, AppUser>()
                .ForMember(dest => dest.UserName, src => src.MapFrom(p => p.Username))
                .ForMember(dest => dest.PasswordHash, src => src.Ignore());

            CreateMap<AppUser, UserServiceModel>()
                .ForMember(dest => dest.Username, src => src.MapFrom(p => p.UserName));
        }
    }
}
