using Contracts.DTOs.Hotel;
using MediatR;

namespace Application.Mediator.Queries.HotelQueries
{
    public class GetHotelByIdQuery : IRequest<HotelDto>
    {
        public int HotelID { get; set; }
    }
}
