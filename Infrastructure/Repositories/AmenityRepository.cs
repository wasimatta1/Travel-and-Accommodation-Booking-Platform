using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
    {
        public AmenityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
