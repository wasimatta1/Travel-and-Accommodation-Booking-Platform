using AutoMapper;
using Contracts.DTOs.Authentication;
using Domain.Entities;

namespace Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequest, User>();
            CreateMap<User, AuthResponse>();
        }
    }
}
