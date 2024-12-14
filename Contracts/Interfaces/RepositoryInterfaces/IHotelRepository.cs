using Domain.Entities;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> SearchHotels(string query, int? starRating, int pageNumber, int pageSize, string[] amenities);
        Task<Hotel> GetHotelPageIdAsync(int hotelId, DateTime checkIn, DateTime checkOut, int adults, int children,
            decimal? priceMin, decimal? priceMax, string? roomType);
    }
}
