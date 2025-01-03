﻿using Application.Mediator.Queries.HotelPageQueries;
using Contracts.DTOs.Cash;
using Contracts.DTOs.HotelPage;
using Contracts.Interfaces;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelPageHandler
{
    public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, IEnumerable<CartItemDto>?>
    {
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetCartItemsQueryHandler> _logger;
        private readonly ICacheService _cacheService;

        public GetCartItemsQueryHandler(ICartService cartService, ILogger<GetCartItemsQueryHandler> logger, IUnitOfWork unitOfWork, ICacheService cacheService)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<CartItemDto>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetCartItemsQueryHandler.Handle called");

            var cartItems = _cartService.GetCartItems();
            if (cartItems == null || !cartItems.Any())
            {
                return new List<CartItemDto>();
            }

            var roomsIds = cartItems.Select(x => x.RoomId).ToList();

            var rooms = await _unitOfWork.Rooms.GetRoomsByIdsAsync(roomsIds);

            _logger.LogInformation("GetCartItemsQueryHandler.Handle returned");

            var roomsDict = rooms.ToDictionary(r => r.RoomID);
            var cashCartDtos = new List<CashCartDto>();

            var cartItemDtos = cartItems
                .Select(x =>
                {
                    if (roomsDict.TryGetValue(x.RoomId, out var room))
                    {
                        var applicableDiscount = room.Discounts
                            .FirstOrDefault(d => d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now);

                        var pricePerNightDiscounted = applicableDiscount != null
                            ? (1 - applicableDiscount.DiscountPercentage / 100) * room.PricePerNight
                            : (decimal?)null;

                        var discountedTotalPrice = applicableDiscount != null
                            ? pricePerNightDiscounted * (x.CheckOutDate - x.CheckInDate).Days
                            : (decimal?)null;

                        // Add to the mutable list
                        cashCartDtos.Add(new CashCartDto
                        {
                            RoomId = room.RoomID,
                            CheckInDate = x.CheckInDate,
                            CheckOutDate = x.CheckOutDate,
                            TotalPrice = discountedTotalPrice ?? room.PricePerNight * (x.CheckOutDate - x.CheckInDate).Days
                        });

                        return new CartItemDto
                        {
                            RoomNumber = room.RoomNumber,
                            PricePerNight = room.PricePerNight,
                            CheckInDate = x.CheckInDate,
                            CheckOutDate = x.CheckOutDate,
                            TotalPrice = room.PricePerNight * (x.CheckOutDate - x.CheckInDate).Days,
                            PricePerNightDescounted = pricePerNightDiscounted,
                            DescountedTotalPrice = discountedTotalPrice
                        };
                    }

                    return null;
                })
                .Where(item => item != null).ToList();


            _cacheService.Set("cashCartDtos", cashCartDtos, TimeSpan.FromMinutes(10));


            return cartItemDtos!;
        }
    }
}
