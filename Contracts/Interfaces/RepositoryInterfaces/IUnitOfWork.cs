namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
        public ICityRepository Cities { get; }
        public IHotelRepository Hotels { get; }
        public IRoomRepository Rooms { get; }

        public Task<int> CompleteAsync();
    }
}
