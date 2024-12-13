using AutoMapper;
using Contracts.DTOs.Home;
using Domain.Entities;

namespace Application.Profiles
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {


            CreateMap<Room, FeaturedDealResponse>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Hotel.City.Country + "," + src.Hotel.City.Name))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Hotel.ThumbnailURL))
            .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Hotel.StarRating))
            .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.PricePerNight))
            .ForMember(dest => dest.PricePerNightDiscounted, opt => opt.MapFrom(src =>
                src.Discounts.Any(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                    ? (1 - src.Discounts.First(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).DiscountPercentage / 100) * src.PricePerNight
                    : src.PricePerNight));
        }

    }
}
