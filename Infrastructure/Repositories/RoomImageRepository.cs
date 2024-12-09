using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class RoomImageRepository : BaseRepository<RoomImage>, IRoomImageRepository
    {
        public RoomImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
