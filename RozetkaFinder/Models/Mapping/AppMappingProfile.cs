using AutoMapper;
using RozetkaFinder.Models.User;
using RozetkaFinder.DTOs;

namespace RozetkaFinder.Models.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User.User, UserRegisterDTO>().ReverseMap();
            CreateMap<User.User, UserInDTO>().ReverseMap();
        }
    }
}
