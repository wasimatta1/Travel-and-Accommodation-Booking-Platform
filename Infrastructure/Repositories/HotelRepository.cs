using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Hotel>> SearchHotels(string query, int? starRating, int pageNumber, int pageSize, string[] amenities)
        {
            var hotelsQurey = _context.Hotels
                .Include(h => h.Amenities)
                .Include(h => h.City)
                .Include(h => h.Rooms)
                .ThenInclude(r => r.bookings)
                .AsQueryable();

            hotelsQurey = hotelsQurey.Where(h => h.Name.Contains(query) || h.Address.Contains(query) || h.City.Name.Contains(query));

            if (starRating != null)
                hotelsQurey = hotelsQurey.Where(h => h.StarRating == starRating);

            if (amenities != null && amenities.Length > 0)
                hotelsQurey = hotelsQurey.Where(h => amenities.All(a => h.Amenities.Any(ha => ha.Name == a)));


            var hotels = await hotelsQurey
                .Select(h => new Hotel
                {
                    HotelID = h.HotelID,
                    Name = h.Name,
                    StarRating = h.StarRating,
                    ThumbnailURL = h.ThumbnailURL,
                    Description = h.Description,
                    Rooms = h.Rooms
                })
                .ToListAsync();

            return hotels;
        }
    }
}
