using AutoMapper;
using Contracts.DTOs.Home;
using Domain.Entities;
namespace Application.Profiles
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            CreateMap<Hotel, SearchResultDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailURL))
            .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.StarRating))
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Rooms.First().RoomType.ToString()))
            .ForMember(dest => dest.HotelDescription, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.Rooms.First().PricePerNight))
                .ForMember(dest => dest.PricePerNightDiscounted, opt => opt.MapFrom(src =>
                    src.Rooms.First().Discounts.Any(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                        ? (1 - src.Rooms.First().Discounts.First(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                        .DiscountPercentage / 100) * src.Rooms.First().PricePerNight : src.Rooms.First().PricePerNight));


            CreateMap<Room, FeaturedDealDto>()
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Hotel.City.Country + "," + src.Hotel.City.Name))
            .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Hotel.ThumbnailURL))
            .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Hotel.StarRating))
            .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.PricePerNight))
            .ForMember(dest => dest.PricePerNightDiscounted, opt => opt.MapFrom(src =>
                src.Discounts.Any(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                    ? (1 - src.Discounts.First(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).DiscountPercentage / 100) * src.PricePerNight
                    : src.PricePerNight));

            CreateMap<Booking, RecentlyVisitedDto>()
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Rooms.First().Hotel.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.Rooms.First().Hotel.ThumbnailURL))
                .ForMember(dest => dest.StarRating, opt => opt.MapFrom(src => src.Rooms.First().Hotel.StarRating))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Rooms.First().Hotel.City.Name))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<City, TrendingDestinationDto>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(src => src.ThumbnailURL));

        }

    }
}
