using Contracts.DTOs.Room;
using MediatR;

namespace Application.Mediator.Queries.RoomQueries
{
    public class GetRoomByIdQuery : IRequest<RoomDto>
    {
        public int RoomID { get; set; }
    }
}
