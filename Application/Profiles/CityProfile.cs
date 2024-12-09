using AutoMapper;
using Contracts.DTOs.City;
using Domain.Entities;

namespace Application.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityDto>();
            CreateMap<UpdateCityDto, City>();
            CreateMap<CreateCityDto, City>();
            CreateMap<UpdateCityDto, CityDto>();
        }
    }
}
