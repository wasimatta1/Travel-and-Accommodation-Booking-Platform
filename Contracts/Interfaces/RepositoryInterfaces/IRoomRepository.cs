using Domain.Entities;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IRoomRepository : IBaseRepository<Room>
    {
        Task<IEnumerable<Room>> GetFeaturedRoomsAsync(int takeNum);
        Task<IEnumerable<Room>> GetRoomsByIdsAsync(IEnumerable<int> roomIds);

    }
}
