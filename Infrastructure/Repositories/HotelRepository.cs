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
        public async Task<Hotel> GetHotelPageIdAsync(int hotelId, DateTime checkIn, DateTime checkOut,
            int adults, int children, decimal? priceMin, decimal? priceMax, string? roomType)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Reviews)
                .FirstOrDefaultAsync(h => h.HotelID == hotelId);

            var rooms = _context.Rooms
                .Include(r => r.bookings)
                .Include(r => r.RoomImages)
                .Include(r => r.Discounts)
                .Where(r => r.HotelID == hotelId)
                .AsEnumerable();

            if (priceMin != null)
                rooms = rooms.Where(r => r.PricePerNight >= priceMin);
            if (priceMax != null)
                rooms = rooms.Where(r => r.PricePerNight <= priceMax);
            if (roomType != null)
                rooms = rooms.Where(r => r.RoomType.ToString() == roomType);


            rooms = rooms
               .Where(r => r.Availability == true
                   && r.AdultsCapacity >= adults
                   && r.ChildrenCapacity >= children
                   && r.bookings != null
                   && r.bookings.OrderBy(b => b.CheckInDate).All(b =>
                       // Check if booking ends before requested check-in or starts after requested check-out
                       (b.CheckOutDate <= checkIn || b.CheckInDate >= checkOut) ||

                       // Check for gaps between consecutive bookings
                       r.bookings.Zip(r.bookings.Skip(1), (current, next) =>
                           next.CheckInDate >= current.CheckOutDate && next.CheckInDate - current.CheckOutDate >= checkOut - checkIn
                       ).All(gap => gap)
                   )
               );


            return new Hotel
            {
                Name = hotel.Name,
                StarRating = hotel.StarRating,
                ImageURL = hotel.ImageURL,
                Description = hotel.Description,
                Reviews = hotel.Reviews,
                LocationGoogelMap = hotel.LocationGoogelMap,
                Rooms = rooms.ToList()
            };

        }
    }
}
