using Domain.Entities;
namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId, int take);
    }
}
