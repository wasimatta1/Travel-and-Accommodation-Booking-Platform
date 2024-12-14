using AutoMapper;
using Contracts.DTOs.Hotel;
using Contracts.DTOs.HotelPage;
using Domain.Entities;

namespace Application.Profiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {

            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.FirstName + " " + src.Owner.LastName))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.RoomCount, opt => opt.MapFrom(src => src.Rooms.Count))
                .ForMember(dest => dest.AmenitiesName, opt => opt.MapFrom(src => src.Amenities.Select(a => a.Name)));

            CreateMap<UpdateHotelDto, Hotel>();
            CreateMap<CreateHotelDto, Hotel>();
            CreateMap<UpdateHotelDto, HotelDto>();

            CreateMap<Hotel, HotelPageDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.GuestReviewsAVG, opt => opt.MapFrom(src => src.Reviews.Average(r => r.Rating)))
                .ForMember(dest => dest.ReviewSample, opt => opt.MapFrom(src => src.Reviews.Select(r => r.Content).FirstOrDefault()))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

        }
    }
}
