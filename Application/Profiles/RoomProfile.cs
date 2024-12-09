using AutoMapper;
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
        }
    }
}
