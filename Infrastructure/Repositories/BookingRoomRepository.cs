using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class BookingRoomRepository : BaseRepository<BookingRoom>, IBookingRoomRepository
    {
        public BookingRoomRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
