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
    }
}
