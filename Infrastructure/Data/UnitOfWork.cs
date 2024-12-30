using Contracts.Interfaces.RepositoryInterfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        private ICityRepository _cities;
        private IHotelRepository _hotels;
        private IRoomRepository _rooms;
        private IRoomImageRepository _roomImages;
        private IBookingRepository _bookings;
        private IBookingRoomRepository _bookingRooms;
        private IHotelAmenityRepository _hotelAmenities;
        private IAmenityRepository _amenities;
        private IDiscountRepository _discounts;
        private IPaymentRepository _payments;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICityRepository Cities => _cities ??= new CityRepository(_context);
        public IHotelRepository Hotels => _hotels ??= new HotelRepository(_context);
        public IRoomRepository Rooms => _rooms ??= new RoomRepository(_context);
        public IRoomImageRepository RoomImages => _roomImages ??= new RoomImageRepository(_context);
        public IBookingRepository Bookings => _bookings ??= new BookingRepository(_context);
        public IBookingRoomRepository BookingRooms => _bookingRooms ??= new BookingRoomRepository(_context);
        public IHotelAmenityRepository HotelAmenities => _hotelAmenities ??= new HotelAmenityRepository(_context);
        public IAmenityRepository Amenities => _amenities ??= new AmenityRepository(_context);
        public IDiscountRepository Discounts => _discounts ??= new DiscountRepository(_context);
        public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                return _currentTransaction;

            _currentTransaction = await _context.Database.BeginTransactionAsync();
            return _currentTransaction;
        }

        public async Task CommitAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No transaction in progress to commit.");

            await _currentTransaction.CommitAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No transaction in progress to roll back.");

            await _currentTransaction.RollbackAsync();
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
