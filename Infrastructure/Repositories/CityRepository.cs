using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> CountNumberOfHotelsInCityAsync(int cityId)
        {
            return await _context.Hotels.CountAsync(h => h.CityID == cityId);
        }
        public async Task<IEnumerable<City>> GetTrendingDestinationsCitiesAsync(int take)
        {
            return await _context.Cities
                 .Include(c => c.Hotels)
                 .ThenInclude(h => h.Rooms)
                 .ThenInclude(r => r.bookings)
                 .OrderByDescending(c => c.Hotels.Sum(h => h.Rooms.Sum(r => r.bookings.Count)))
                 .Take(take)
                 .Select(c => new City
                 {
                     Name = c.Name,
                     ThumbnailURL = c.ThumbnailURL
                 })
                 .ToListAsync();
        }
    }
}
