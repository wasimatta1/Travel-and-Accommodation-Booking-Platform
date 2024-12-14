using AutoMapper;
using Contracts.DTOs.HotelPage;
using Contracts.DTOs.Room;
using Domain.Entities;

namespace Application.Profiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
                .ForMember(dest => dest.ImagesUrl, opt => opt.MapFrom(src => src.RoomImages.Select(r => r.ImageUrl)))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()));

            CreateMap<CreateRoomDto, Room>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType));
            CreateMap<UpdateRoomDto, Room>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType));
            CreateMap<UpdateRoomDto, RoomDto>();


            CreateMap<Room, RoomPageDto>()
                .ForMember(dest => dest.ImagesUrl, opt => opt.MapFrom(src => src.RoomImages.Select(r => r.ImageUrl)))
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.PricePerNight))
                .ForMember(dest => dest.DiscountedPrice, opt => opt.MapFrom(src =>
                    src.Discounts.Any(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                        ? (1 - src.Discounts.First(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now).DiscountPercentage / 100) * src.PricePerNight
                        : src.PricePerNight));
        }
    }
}
