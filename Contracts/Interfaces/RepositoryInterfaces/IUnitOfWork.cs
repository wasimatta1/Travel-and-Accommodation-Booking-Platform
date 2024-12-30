using Microsoft.EntityFrameworkCore.Storage;

namespace Contracts.Interfaces.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
        public ICityRepository Cities { get; }
        public IHotelRepository Hotels { get; }
        public IRoomRepository Rooms { get; }
        public IRoomImageRepository RoomImages { get; }
        public IBookingRepository Bookings { get; }
        public IBookingRoomRepository BookingRooms { get; }
        public IHotelAmenityRepository HotelAmenities { get; }
        public IAmenityRepository Amenities { get; }
        public IDiscountRepository Discounts { get; }
        public IPaymentRepository Payments { get; }
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public Task CommitAsync();
        public Task RollbackAsync();
        public Task<int> CompleteAsync();
    }
}
