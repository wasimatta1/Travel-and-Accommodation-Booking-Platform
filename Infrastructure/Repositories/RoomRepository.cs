using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetFeaturedRoomsAsync(int takeNum)
        {
            return await _context.Rooms
                .Include(r => r.Discounts)
                .Include(r => r.Hotel)
                .ThenInclude(h => h.City)
                .Where(r => r.Discounts.Any(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now))
                .GroupBy(r => r.HotelID)
                .Select(g => g.First())
                .Take(takeNum)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetRoomsByIdsAsync(IEnumerable<int> roomIds)
        {
            return await _context.Rooms
                .Include(r => r.Discounts)
                .Where(r => roomIds.Contains(r.RoomID))
                .ToListAsync();
        }
    }
}
