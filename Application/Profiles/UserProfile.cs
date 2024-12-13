using AutoMapper;
using Contracts.DTOs.Authentication;
using Domain.Entities;

namespace Application.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequestDto, User>();
            CreateMap<User, AuthResponseDto>();
        }
    }
}
