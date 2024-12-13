using Domain.Entities;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<int> CountNumberOfHotelsInCityAsync(int cityId);
        Task<IEnumerable<City>> GetTrendingDestinationsCitiesAsync(int take);

    }
}
