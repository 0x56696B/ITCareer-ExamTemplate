using AutoMapper;
using Project.Services.Models.User;
using Project.Web.Models;

namespace Project.Web.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<UserServiceModel, UserWebModel>();
            CreateMap<UserWebModel, UserServiceModel>();

            CreateMap<LoginWebModel, LoginServiceModel>();
            CreateMap<RegisterWebModel, RegisterServiceModel>();
        }
    }
}
