using Application.Mediator.Commands.CheckoutCommands;
using AutoMapper;
using Contracts.DTOs.Cash;
using Contracts.DTOs.Checkout;
using Contracts.DTOs.Room;
using Contracts.Interfaces;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Mediator.Handlers.CheckoutHandler
{
    public class ProcessCheckoutCommandHandler : IRequestHandler<ProcessCheckoutCommand, BookingConfirmationDto?>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly ILogger<ProcessCheckoutCommandHandler> _logger;
        private readonly ICacheService _cacheService;
        private readonly UserManager<User> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public ProcessCheckoutCommandHandler(
            ILogger<ProcessCheckoutCommandHandler> logger,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ICartService cartService,
            ICacheService cacheService,
            UserManager<User> userManger,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _cacheService = cacheService;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<BookingConfirmationDto?> Handle(ProcessCheckoutCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ProcessCheckoutCommandHandler.Handle called");

            var cashCartItems = _cacheService.Get<IEnumerable<CashCartDto>>("cashCartDtos");
            if (cashCartItems == null || !cashCartItems.Any())
            {
                _logger.LogWarning("No items found in cache for checkout.");
                return null;
            }

            var user = await GetUserFromTokenAsync();
            if (user == null)
                return null;

            var firstItem = cashCartItems.First();
            var room = await _unitOfWork.Rooms.GetByIdAsync(firstItem.RoomId);
            if (room == null)
            {
                _logger.LogWarning($"Room with ID {firstItem.RoomId} not found.");
                return null;
            }

            var hotel = await _unitOfWork.Hotels.GetByIdAsync(room.HotelID);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with ID {room.HotelID} not found.");
                return null;
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var booking = await CreateBookingAsync(user, firstItem);

                    var bookingConfirmationDto = InitializeBookingConfirmationDto(hotel);

                    foreach (var item in cashCartItems)
                    {
                        await ProcessBookingRoomAsync(item, booking, bookingConfirmationDto);
                    }

                    var payment = await CreatePaymentAsync(request, booking);

                    booking.PaymentID = payment.PaymentID;
                    await _unitOfWork.CompleteAsync();

                    bookingConfirmationDto.ConfirmationNumber = booking.BookingID.ToString();
                    bookingConfirmationDto.TotalPrice = booking.TotalPrice;
                    bookingConfirmationDto.CheckInDate = firstItem.CheckInDate;
                    bookingConfirmationDto.CheckOutDate = firstItem.CheckOutDate;

                    await transaction.CommitAsync();

                    string invoiceDetails =
                         $"Hotel Name: {hotel.Name}\n" +
                         $"Check In Date: {firstItem.CheckInDate}\n" +
                         $"Check Out Date: {firstItem.CheckOutDate}\n" +
                         $"Total Price: {booking.TotalPrice}\n" +
                         $"Payment Method: {payment.PaymentMethod}\n";

                    await SendBookingConfirmationEmailAsync(user.Email!, "Booking Confirmation", "Your booking has been confirmed " +
                        "with the following details:\n" + invoiceDetails);

                    return bookingConfirmationDto;
                }
                catch (Exception ex)
                {
                    await SendBookingConfirmationEmailAsync(user.Email!, "Booking Failed", "Your booking has failed. Please Check " +
                        "your payment details or balance and try again.");

                    _logger.LogError(ex, "An error occurred during checkout. Rolling back transaction.");
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private async Task<User?> GetUserFromTokenAsync()
        {
            var email = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("User email was not found");
                return null;
            }

            var user = await _userManger.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email: {email} was not found");
            }

            return user;
        }

        private async Task<Booking> CreateBookingAsync(User user, CashCartDto firstItem)
        {
            var booking = new Booking
            {
                UserID = user.Id,
                CheckInDate = firstItem.CheckInDate,
                CheckOutDate = firstItem.CheckOutDate,
                TotalPrice = 0
            };

            await _unitOfWork.Bookings.CreateAsync(booking);
            await _unitOfWork.CompleteAsync();

            return booking;
        }

        private BookingConfirmationDto InitializeBookingConfirmationDto(Hotel hotel)
        {
            return new BookingConfirmationDto
            {
                HotelName = hotel.Name,
                HotelAddress = hotel.Address,
                BookingDtos = new List<BookingDto>()
            };
        }

        private async Task ProcessBookingRoomAsync(CashCartDto item, Booking booking, BookingConfirmationDto bookingConfirmationDto)
        {
            var bookingRoom = new BookingRoom
            {
                BookingID = booking.BookingID,
                RoomID = item.RoomId
            };

            await _unitOfWork.BookingRooms.CreateAsync(bookingRoom);
            await _unitOfWork.CompleteAsync();

            booking.TotalPrice += item.TotalPrice;

            var room = await _unitOfWork.Rooms.GetByIdAsync(item.RoomId);
            var bookingDto = new BookingDto
            {
                RoomDetails = _mapper.Map<RoomDetailsDto>(room),
                TotalPriceForRoom = item.TotalPrice
            };

            bookingConfirmationDto.BookingDtos.Add(bookingDto);
        }

        private async Task<Payment> CreatePaymentAsync(ProcessCheckoutCommand request, Booking booking)
        {
            string paymentMethod = request.CheckoutInfo.CardNumber.Trim().Substring(0, 3);
            paymentMethod = Enum.Parse(typeof(PaymentMethod), paymentMethod).ToString()!;

            var payment = new Payment
            {
                BookingID = booking.BookingID,
                PaymentMethod = paymentMethod,
                TotalPrice = booking.TotalPrice
            };

            await _unitOfWork.Payments.CreateAsync(payment);
            await _unitOfWork.CompleteAsync();

            return payment;
        }
        private async Task SendBookingConfirmationEmailAsync(string email, string subject, string message)
        {
            await _emailService.SendEmailAsync(email, subject, message);
        }
    }
}
