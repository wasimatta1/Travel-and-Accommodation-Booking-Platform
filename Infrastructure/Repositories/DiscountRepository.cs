using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Discount>> GetDiscountsByRoomIdAsync(IEnumerable<int> roomId)
        {
            return await _context.Discounts.Where(d =>
                roomId.Contains(d.RoomId) &&
                d.StartDate <= DateTime.Now
                && d.EndDate >= DateTime.Now)
                .ToListAsync();
        }
    }
}
