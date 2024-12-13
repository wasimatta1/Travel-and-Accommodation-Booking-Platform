using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId, int take)
        {

            return await _context.Bookings
                .Include(b => b.Rooms)
                .ThenInclude(r => r.Hotel)
                .ThenInclude(h => h.City)
                .Where(b => b.UserID == userId)
                .GroupBy(b => b.Rooms.First().HotelID)
                .Select(g => g.First())
                .Take(take)
                .ToListAsync();
        }


    }
}
