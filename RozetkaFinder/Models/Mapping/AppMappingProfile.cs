using AutoMapper;
using RozetkaFinder.Models.User;
using RozetkaFinder.Models.GoodObjects;
using RozetkaFinder.DTOs;

namespace RozetkaFinder.Models.Mapping
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<User.User, UserRegisterDTO>().ReverseMap();
            CreateMap<User.User, UserInDTO>().ReverseMap();
            CreateMap<SubscribtionGood, GoodDTO>().ReverseMap();
            CreateMap<Good, SubscribtionGood>().ReverseMap();
            CreateMap<Good, GoodDTO>().ReverseMap();

        }
    }
}
