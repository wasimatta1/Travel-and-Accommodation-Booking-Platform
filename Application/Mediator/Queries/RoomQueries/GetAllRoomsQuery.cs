using Contracts.DTOs.Room;
using MediatR;

namespace Application.Mediator.Queries.RoomQueries
{
    public class GetAllRoomsQuery : IRequest<IEnumerable<RoomDto>>
    {
        public string? RoomNumber { get; set; }
        public string? Type { get; set; }
        public int? AdultCapacity { get; set; }
        public int? ChildrenCapacity { get; set; }
        public bool? Availability { get; set; }
        public int PagNumber { get; set; }
        public int PageSize { get; set; }

    }
}
