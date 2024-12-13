using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class HotelAmenityRepository : BaseRepository<HotelAmenity>, IHotelAmenityRepository
    {
        public HotelAmenityRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
