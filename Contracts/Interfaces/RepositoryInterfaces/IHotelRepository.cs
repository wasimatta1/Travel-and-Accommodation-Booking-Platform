using Domain.Entities;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<IEnumerable<Hotel>> SearchHotels(string query, int pageNumber, int pageSize, string[] amenities);

    }
}
