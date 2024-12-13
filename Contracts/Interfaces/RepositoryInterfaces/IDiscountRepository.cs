using Domain.Entities;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IDiscountRepository : IBaseRepository<Discount>
    {
        Task<IEnumerable<Discount>> GetDiscountsByRoomIdAsync(IEnumerable<int> roomId);
    }
}
