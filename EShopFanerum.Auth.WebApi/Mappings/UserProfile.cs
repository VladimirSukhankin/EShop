using AutoMapper;
using EShopFanerum.Auth.WebApi.Models;
using EShopFanerum.Domain.Entites;
using EShopFanerum.Domain.Entites.Auth;

namespace EShopFanerum.Auth.WebApi.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserModel, User>();
        CreateMap<User, UserModel>();
        CreateMap<RegisterModel, User>();
        CreateMap<UpdateModel, User>();
        CreateMap<RefreshToken, RefreshTokenModel>();
    }
}